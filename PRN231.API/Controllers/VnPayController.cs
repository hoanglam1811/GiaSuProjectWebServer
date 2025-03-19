using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repository.Interfaces;
using PRN231.Services;
using PRN231.Services.Interfaces;
using System.Net;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnPayController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVnPayService _vnPayService;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericService<Transaction, TransactionDTO> _transactionService;
        private readonly UserManager<User> _manager;

        public VnPayController(IHttpContextAccessor httpContextAccessor, IVnPayService vnPayService,
            IGenericRepository<User> userRepo, UserManager<User> manager,
            IGenericService<Transaction, TransactionDTO> transactionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayService = vnPayService;
            _userRepo = userRepo;
            _transactionService = transactionService;
            _manager = manager;
        }

        [HttpPost("Pay")]
        public async Task<ActionResult<ApiResponse>> CreatePayment([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            try
            {
                string payload = _vnPayService.CreatePayment(createPaymentRequest);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, payload));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPost("TransferMoney")]
        public async Task<ActionResult<ApiResponse>> TransferMoney([FromBody] TransferPaymentUserRequest createPaymentRequest)
        {
            //try
            //{
                var user = await _userRepo.Get(createPaymentRequest.UserId);
                //var receiver = await _userRepo.Get(createPaymentRequest.ReceiverId);
                if (user == null)
                {
                    return NotFound($"User with ID {createPaymentRequest.UserId} not found.");
                }
                if (user.Credit < createPaymentRequest.Amount)
                {
                    return BadRequest("Not enough credit");
                }

                var adminUsers = await _manager.GetUsersInRoleAsync("Admin");
                var admin = adminUsers.FirstOrDefault();
                if(admin == null) return BadRequest("Admin not found");

                var transaction = new TransactionDTO{
                    UserId = createPaymentRequest.UserId,
                    ReceiverId = admin.Id,
                    Amount = createPaymentRequest.Amount,
                    TransactionCode = "TRANSFER",
                    Message = "Transfer credit to admin",
                    Type = TransactionConstant.PURCHASE,
                    Status = StatusConstant.ACTIVE,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                await _transactionService.Add(transaction);

                user.Credit -= createPaymentRequest.Amount;
                admin.Credit += createPaymentRequest.Amount;

                await _userRepo.Update(user);
                await _userRepo.Update(admin);

                return Ok();

                //string payload = _vnPayService.CreatePayment(createPaymentRequest);

                //return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, payload));
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            //}
        }

        [HttpPost("TransferMoneyTutor")]
        public async Task<ActionResult<ApiResponse>> TransferMoneyTutor([FromBody] CreatePaymentUserRequest createPaymentRequest)
        {
            try
            {
                var user = await _userRepo.Get(createPaymentRequest.UserId);
                var adminUsers = await _manager.GetUsersInRoleAsync("Admin");
                var admins = adminUsers.FirstOrDefault();

                if(admins == null) return BadRequest("Admin not found");

                var admin = await _userRepo.Get(admins.Id);
                //var receiver = await _userRepo.Get(createPaymentRequest.ReceiverId);
                if (admin.Credit < createPaymentRequest.Amount)
                {
                    return BadRequest("Not enough credit");
                }

                var transaction = new TransactionDTO{
                    UserId = admin.Id,
                    ReceiverId = user.Id,
                    TransactionCode = "TRANSFER",
                    Amount = (decimal)(createPaymentRequest.Amount - (decimal)(createPaymentRequest.Amount*20/100)),
                    Message = "Transfer credit to tutor",
                    Type = TransactionConstant.TRANSFER,
                    Status = StatusConstant.ACTIVE,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                await _transactionService.Add(transaction);

                user.Credit += createPaymentRequest.Amount - (decimal)(createPaymentRequest.Amount*20/100);
                admin.Credit -= createPaymentRequest.Amount - (decimal)(createPaymentRequest.Amount*20/100);

                await _userRepo.Update(user);
                await _userRepo.Update(admin);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPost("CheckUserCredit")]
        public async Task<ActionResult<ApiResponse>> CheckPaymentForUserCredit([FromBody] CreatePaymentUserRequest createPaymentRequest)
        {
            try
            {
                var user = await _userRepo.Get(createPaymentRequest.UserId);
                if (user == null)
                {
                    return NotFound($"User with ID {createPaymentRequest.UserId} not found.");
                }
                if (user.Credit < createPaymentRequest.Amount)
                {
                    return BadRequest(createPaymentRequest.Amount- user.Credit);
                }
                else
                {
                    return Ok();
                }
                //return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, payload));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPost("PayUserCredit")]
        public async Task<ActionResult<ApiResponse>> CreatePaymentForUserCredit([FromBody] CreatePaymentUserRequest createPaymentRequest)
        {
            try
            {
                string payload = _vnPayService.CreatePaymentForUserCredit(createPaymentRequest);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, payload));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpGet("Result/{Id}")]
        public async Task<ActionResult<ApiResponse>> GetPaymentResultUser(int Id)
        {
            var context = _httpContextAccessor.HttpContext;

            try
            {
                int result = _vnPayService.GetPaymentResult();

                string orderId = context.Request.Query["vnp_TxnRef"];
                string amount = context.Request.Query["vnp_Amount"];
                string orderInfo = context.Request.Query["vnp_OrderInfo"];
                string payDate = context.Request.Query["vnp_PayDate"];
                string transactionCode = context.Request.Query["vnp_TransactionNo"];
                //string userId = context.Request.Query["vnp_Billing_Email"];

                var user = await _userRepo.Get(Id);
                var roleList = await _manager.GetRolesAsync(user);
                var role = roleList.FirstOrDefault() ?? "";
                if (user != null && result == 1){
                    var transaction = new TransactionDTO{
                        UserId = Id,
                        ReceiverId = Id,
                        Amount = decimal.Parse(amount)/100,
                        TransactionCode = transactionCode,
                        Message = "Charge credit for user",
                        Type = TransactionConstant.CHARGE,
                        Status = StatusConstant.ACTIVE,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    await _transactionService.Add(transaction);

                    user.Credit = user.Credit + decimal.Parse(amount)/100;
                    await _userRepo.Update(user);
                }
                if(role == "Tutor")
                {
                    return Redirect("http://localhost:3000/ProfileTutor");
                }
                return Redirect("http://localhost:3000/Profile");
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }

        }

        [HttpGet("Result")]
        public async Task<ActionResult<ApiResponse>> GetPaymentResult()
        {
            var context = _httpContextAccessor.HttpContext;

            try
            {
                int result = _vnPayService.GetPaymentResult();

                string orderId = context.Request.Query["vnp_TxnRef"];
                string amount = context.Request.Query["vnp_Amount"];
                string orderInfo = context.Request.Query["vnp_OrderInfo"];
                string payDate = context.Request.Query["vnp_PayDate"];
                string userId = context.Request.Query["vnp_Billing_Email"];

                CreatePaymentResponse createPaymentResponse = new CreatePaymentResponse
                {
                    OrderId = orderId,
                    Amount = amount,
                    OrderInfo = orderInfo,
                    PayDate = payDate
                };

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, createPaymentResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }

        }
    }
}
