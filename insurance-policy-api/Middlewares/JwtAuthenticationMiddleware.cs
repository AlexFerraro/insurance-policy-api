//using System.Text;

//namespace insurance_policy_api.Middlewares;

//public class JwtAuthenticationMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
//    private readonly string _secretKey = "sua_chave_secreta_aqui";

//    public JwtAuthenticationMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//        if (token != null)
//        {
//            try
//            {
//                var tokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ClockSkew = TimeSpan.Zero
//                };

//                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

//                context.User = claimsPrincipal;

//            }
//            catch (Exception)
//            {
//                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                await context.Response.WriteAsync("Token inválido");
//                return;
//            }
//        }

//        await _next(context);
//    }
//}
