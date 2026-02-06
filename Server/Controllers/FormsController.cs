using DynamicFormsApp.Server.Services;
using DynamicFormsApp.Shared.Models;
using DynamicFormsApp.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicFormsApp.Server.Controllers
{
    [Route("api/forms")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly DynamicFormService _svc;
        private readonly IUserService _userSvc;
        private readonly IEmailService _emailSvc;

        public FormsController(DynamicFormService svc, IUserService userSvc, IEmailService emailSvc)
        {
            _svc = svc;
            _userSvc = userSvc;
            _emailSvc = emailSvc;
        }

        // POST /api/forms
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFormDto dto)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var newFormId = await _svc.CreateFormAsync(dto.Name, dto.Description, dto.Fields, user, dto.RequireLogin, dto.NotifyOnResponse, dto.NotificationEmail, dto.IsActive, dto.IsDraft);
            return Ok(new { FormId = newFormId });
        }

        // PUT /api/forms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateFormDto dto)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var newFormId = await _svc.UpdateFormAsync(id, dto, user);
            return Ok(new { FormId = newFormId });
        }


        // GET /api/forms/{id}/responses
        [HttpGet("{id}/responses")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetResponses(int id)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }
            var form = await _svc.GetFormAsync(id);
            if (!form.IsActive)
            {
                var owner = await _userSvc.GetUserData(form.CreatedBy);
                Response.Headers["Cache-Control"] = "no-store";
                return StatusCode(410, new
                {
                    Message = "This form has been deleted. Please contact the owner.",
                    Owner = owner?.DisplayName ?? form.CreatedBy,
                    Email = owner?.Email
                });
            }

            var rows = await _svc.GetResponsesAsync(id, user);
            return Ok(rows);
        }

        [HttpGet("{id}/responses/{responseId}")]
        public async Task<ActionResult<Dictionary<string, object>>> GetResponse(int id, int responseId)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var form = await _svc.GetFormAsync(id);
            if (!form.IsActive)
            {
                var owner = await _userSvc.GetUserData(form.CreatedBy);
                Response.Headers["Cache-Control"] = "no-store";
                return StatusCode(410, new
                {
                    Message = "This form has been deleted. Please contact the owner.",
                    Owner = owner?.DisplayName ?? form.CreatedBy,
                    Email = owner?.Email
                });
            }

            var row = await _svc.GetResponseAsync(id, responseId, user);
            return Ok(row);
        }


        // GET /api/forms/{id}
        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<Form>> Get(int id)
        {
            var form = await _svc.GetFormAsync(id);
            if (!form.IsActive)
            {
                var owner = await _userSvc.GetUserData(form.CreatedBy);
                Response.Headers["Cache-Control"] = "no-store";
                return StatusCode(410, new
                {
                    Message = "This form has been deleted. Please contact the owner.",
                    Owner = owner?.DisplayName ?? form.CreatedBy,
                    Email = owner?.Email
                });
            }
            return Ok(form);
        }

        [HttpGet("{id}/history")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<IEnumerable<Form>>> History(int id)
        {
            var current = await _svc.GetFormAsync(id);
            if (!current.IsActive)
            {
                var owner = await _userSvc.GetUserData(current.CreatedBy);
                Response.Headers["Cache-Control"] = "no-store";
                return StatusCode(410, new
                {
                    Message = "This form has been deleted. Please contact the owner.",
                    Owner = owner?.DisplayName ?? current.CreatedBy,
                    Email = owner?.Email
                });
            }
            var history = await _svc.GetFormHistoryAsync(id);
            return Ok(history);
        }
        // GET /api/forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Form>>> GetAll()
        {
            var all = await _svc.GetAllFormsAsync();
            return Ok(all);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            var count = await _svc.GetFormCountAsync();
            return Ok(count);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Form>>> Search([FromQuery] bool includePrivate = false)
        {
            var loggedIn = Request.Cookies.TryGetValue("userName", out var user) && !string.IsNullOrEmpty(user);
            var results = await _svc.SearchFormsAsync(loggedIn && includePrivate);
            return Ok(results);
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<Form>>> GetMine()
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var mine = await _svc.GetFormsByUserAsync(user);
            return Ok(mine);
        }

        [HttpGet("drafts")]
        public async Task<ActionResult<IEnumerable<Form>>> GetDrafts()
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var drafts = await _svc.GetDraftFormsByUserAsync(user);
            return Ok(drafts);
        }

        [HttpGet("shared")]
        public async Task<ActionResult<IEnumerable<Form>>> GetSharedWithMe()
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var shared = await _svc.GetFormsSharedWithUserAsync(user);
            return Ok(shared);
        }

        // DELETE /api/forms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            await _svc.DeleteFormAsync(id, user);
            return NoContent();
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            await _svc.DeactivateFormAsync(id, user);
            return NoContent();
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            await _svc.ActivateFormAsync(id, user);
            return NoContent();
        }

        [HttpPost("{id}/share")]
        public async Task<IActionResult> Share(int id, [FromBody] ShareFormDto dto)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            await _svc.ShareFormAsync(id, user, dto.UserName);
            return NoContent();
        }

        [HttpGet("{id}/shares")]
        public async Task<ActionResult<IEnumerable<FormShare>>> Shares(int id)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            var shares = await _svc.GetFormSharesAsync(id, user);
            return Ok(shares);
        }

        [HttpDelete("{id}/shares/{target}")]
        public async Task<IActionResult> RemoveShare(int id, string target)
        {
            if (!Request.Cookies.TryGetValue("userName", out var user) || string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            await _svc.RemoveShareAsync(id, user, target);
            return NoContent();
        }

        // POST /api/forms/{id}/responses
        [HttpPost("{id}/responses")]

        public async Task<IActionResult> Submit(int id, [FromBody] Dictionary<string, object> values)
        {
            try
            {
                string? responder = null;
                var form = await _svc.GetFormAsync(id);
                if (!form.IsActive)
                {
                    var owner = await _userSvc.GetUserData(form.CreatedBy);
                    Response.Headers["Cache-Control"] = "no-store";
                    return StatusCode(410, new
                    {
                        Message = "This form has been deleted. Please contact the owner.",
                        Owner = owner?.DisplayName ?? form.CreatedBy,
                        Email = owner?.Email
                    });
                }
                if (form.RequireLogin && Request.Cookies.TryGetValue("userName", out var userId) && !string.IsNullOrEmpty(userId))
                {
                    var userData = await _userSvc.GetUserData(userId);
                    responder = userData?.DisplayName ?? userId;
                }

                form = await _svc.StoreResponseAsync(id, values, responder);

                if (form.NotifyOnResponse)
                {
                    string? to = form.NotificationEmail;
                    if (string.IsNullOrWhiteSpace(to))
                    {
                        var user = await _userSvc.GetUserData(form.CreatedBy);
                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            to = user.Email;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(to))
                    {
                        await _emailSvc.SendFormResponseNotification(to, form.Name, form.Id);
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                // Return full exception details in development
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    Details = ex.ToString()
                });
            }
        }
    }
}
