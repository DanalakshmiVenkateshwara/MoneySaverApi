using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;

namespace MoneySaverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly FirebaseAuth _auth;
        private readonly FirebaseMessaging _messaging;
        private readonly ILogger<FirebaseController> _logger;

        public FirebaseController(ILogger<FirebaseController> logger)
        {
            _auth = FirebaseAuth.DefaultInstance;
            _messaging = FirebaseMessaging.DefaultInstance;
            _logger = logger;
        }

        // User CRUD operations

 [HttpPost("createUser")]
public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserModel model)
{
    try
    {
        UserRecord user;

        // Check if user exists by phone number
        try
        {
            user = await _auth.GetUserByPhoneNumberAsync(model.PhoneNumber);
        }
        catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.UserNotFound)
        {
            // User does not exist, so create a new one
            user = await _auth.CreateUserAsync(new UserRecordArgs
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                DisplayName = model.DisplayName,
                PhotoUrl = model.PhotoUrl
            });
        }

        // If user already exists by phone number, update the email if necessary
        if (user != null)
        {
            // Check if email needs to be updated
            if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
            {
                // First, we need to ensure the email is not already used by another user
                try
                {
                    var existingUser = await _auth.GetUserByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                         var rcustomToken = await _auth.CreateCustomTokenAsync(existingUser.Uid);
        return Ok(new { message = "User exists, token generated successfully.", authToken = rcustomToken });
                      
                    }
                }
                catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.UserNotFound)
                {
                    // Email does not exist, we can proceed to update it
                }

                // Update user data with the new email
                var updateArgs = new UserRecordArgs
                {
                    Uid = user.Uid,
                    Email = model.Email,
                    DisplayName = model.DisplayName,
                    PhotoUrl = model.PhotoUrl
                };

                // Update user data
                await _auth.UpdateUserAsync(updateArgs);
            }
        }

        // Set custom claims (if any)
        await _auth.SetCustomUserClaimsAsync(user.Uid, new Dictionary<string, object>
        {
            { "tenantId", model.TenantId }
        });

        // Generate a Firebase custom auth token for the user
        var customToken = await _auth.CreateCustomTokenAsync(user.Uid);

        return Ok(new { message = "User created or retrieved successfully.", authToken = customToken });
    }
    catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.PhoneNumberAlreadyExists)
    {
        return BadRequest("The phone number is already associated with an existing user.");
    }
    catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.EmailAlreadyExists)
    {
        // If the email already exists, still generate a token for the existing user
        var existingUser = await _auth.GetUserByEmailAsync(model.Email);
        var customToken = await _auth.CreateCustomTokenAsync(existingUser.Uid);
        return Ok(new { message = "User exists, token generated successfully.", authToken = customToken });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating or retrieving user");
        return BadRequest(ex.Message);
    }
}






        [HttpGet("getUser/{uid}")]
        public async Task<IActionResult> GetUserAsync(string uid)
        {
            try
            {
                var userRecord = await _auth.GetUserAsync(uid);
                return Ok(userRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateUser/{uid}")]
        public async Task<IActionResult> UpdateUserAsync(string uid, [FromBody] UpdateUserModel model)
        {
            try
            {
                UserRecordArgs args = new UserRecordArgs
                {
                    Uid = uid,
                    Email = model.Email,
                    DisplayName = model.DisplayName
                };
                var userRecord = await _auth.UpdateUserAsync(args);
                return Ok(userRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteUser/{uid}")]
        public async Task<IActionResult> DeleteUserAsync(string uid)
        {
            try
            {
                await _auth.DeleteUserAsync(uid);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return BadRequest(ex.Message);
            }
        }

        // FCM features

        [HttpPost("sendNotificationToDevice")]
        public async Task<IActionResult> SendNotificationToDeviceAsync([FromBody] NotificationToDeviceModel model)
        {
            try
            {
                var message = new Message()
                {
                    Token = model.DeviceToken,
                    Notification = new Notification
                    {
                        Title = model.Title,
                        Body = model.Body
                    }
                };
                var response = await _messaging.SendAsync(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to device");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sendNotificationToTopic")]
        public async Task<IActionResult> SendNotificationToTopicAsync([FromBody] NotificationToTopicModel model)
        {
            try
            {
                var message = new Message()
                {
                    Topic = model.Topic,
                    Notification = new Notification
                    {
                        Title = model.Title,
                        Body = model.Body
                    }
                };
                var response = await _messaging.SendAsync(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to topic");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("subscribeToTopic")]
        public async Task<IActionResult> SubscribeToTopicAsync([FromBody] SubscriptionModel model)
        {
            try
            {
                var response = await _messaging.SubscribeToTopicAsync(model.DeviceTokens, model.Topic);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to topic");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("unsubscribeFromTopic")]
        public async Task<IActionResult> UnsubscribeFromTopicAsync([FromBody] SubscriptionModel model)
        {
            try
            {
                var response = await _messaging.UnsubscribeFromTopicAsync(model.DeviceTokens, model.Topic);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing from topic");
                return BadRequest(ex.Message);
            }
        }
    }

    public class CreateUserModel
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
        public string TenantId { get; set; }
    }


    public class UpdateUserModel
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }

    public class NotificationToDeviceModel
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class NotificationToTopicModel
    {
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class SubscriptionModel
    {
        public string[] DeviceTokens { get; set; }
        public string Topic { get; set; }
    }
}
