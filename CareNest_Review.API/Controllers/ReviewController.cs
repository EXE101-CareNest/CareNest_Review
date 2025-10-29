using CareNest_Review.Application.Common;
using CareNest_Review.Application.Features.Commands.Create;
using CareNest_Review.Application.Features.Commands.Delete;
using CareNest_Review.Application.Features.Commands.Update;
using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Features.Queries.GetById;
using CareNest_Review.Application.Interfaces.CQRS;
using CareNest_Review.Domain.Commons.Constant;
using CareNest_Review.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace CareNest_Review.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public ReviewController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortDirection"></param>
        /// <param name="ServiceDetailId"></param>
        /// <param name="ProductDetailId"></param>
        /// <param name="Type">1 = service, 2 = product, 3 = order</param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? serviceDetailId = null,
            [FromQuery] string? productDetailId = null,
            [FromQuery] int? type = null,
            [FromQuery] string? customerId = null
            )
        {
            var query = new GetAllPagingQuery()
            {
                Index = pageIndex,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                CustomerId = customerId,
                ProductDetailId = productDetailId,
                ServiceDetailId = serviceDetailId,
                Type = type
            };
            var result = await _dispatcher.DispatchQueryAsync<GetAllPagingQuery, PageResult<ReviewResponse>>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// Hiển thị chi tiết đánh giá theo id
        /// </summary>
        /// <param name="id">Id đánh giá</param>
        /// <returns>chi tiết đánh giá</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdQuery() { Id = id };
            ReviewResponse result = await _dispatcher.DispatchQueryAsync<GetByIdQuery, ReviewResponse>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// tạo mới đánh giá
        /// </summary>
        /// <param name="command">thông tin đánh giá</param>
        /// <returns>thông tin đánh giá mới tạo xog</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommand command)
        {
            ReviewResponse result = await _dispatcher.DispatchAsync<CreateCommand, ReviewResponse>(command);

            return this.OkResponse(result, MessageConstant.SuccessCreate);
        }

        /// <summary>
        /// Cập nhật thông tin đánh giá
        /// </summary>
        /// <param name="id">Id đánh giá</param>
        /// <param name="request">các thông tin cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRequest request)
        {

            var command = new UpdateCommand()
            {
                Id = id,
                Contents = request.Contents,
                ImgUrl = request.ImgUrl,
                Rating = request.Rating
            };
                ReviewResponse result = await _dispatcher.DispatchAsync<UpdateCommand, ReviewResponse>(command);

            return this.OkResponse(result, MessageConstant.SuccessUpdate);
        }

        /// <summary>
        /// xoá đánh giá
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dispatcher.DispatchAsync(new DeleteCommand { Id = id });
            return this.OkResponse(MessageConstant.SuccessDelete);
        }

        
    }
}
