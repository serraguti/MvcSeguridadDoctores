using Microsoft.AspNetCore.Authorization;

namespace MvcSeguridadDoctores.Policies
{
    public class OverSalarioRequirement :
        AuthorizationHandler<OverSalarioRequirement>,
        IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context, 
            OverSalarioRequirement requirement)
        {
            //PODEMOS PREGUNTAR SI EXISTE EL CLAIM
            if (context.User.HasClaim(x => x.Type == "SALARIO") == false)
            {
                context.Fail();
            }
            else
            {
                string data = context.User.FindFirst("SALARIO").Value;
                int salario = int.Parse(data);
                //PREGUNTAMOS POR EL VALOR PARA PODER ACCEDER
                if (salario >= 220000)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }
}
