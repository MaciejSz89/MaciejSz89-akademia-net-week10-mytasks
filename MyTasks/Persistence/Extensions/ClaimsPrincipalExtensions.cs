using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace MyTasks.Persistence.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal model)
        {
            return model.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
