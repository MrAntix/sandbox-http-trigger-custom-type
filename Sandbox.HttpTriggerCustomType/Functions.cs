using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace HttpTriggerCustomType
{
    public static class Functions
    {
        [Function(nameof(TriggerModelAsync))]
        public static async Task<IActionResult> TriggerModelAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ROUTES.TRIGGER)] Model model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return new OkResult();
        }

        [Function(nameof(BindModelAsync))]
        public static async Task<IActionResult> BindModelAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ROUTES.BIND)] HttpRequest request,
            [FromBody] Model model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return new OkResult();
        }
    }
}
