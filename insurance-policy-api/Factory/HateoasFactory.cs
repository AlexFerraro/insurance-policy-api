using insurance_policy_api.DTOs;

namespace insurance_policy_api.Factory;

public class HateoasFactory
{
    public LinkDTO[] CreateLinksForCreatedPolicy(long id, string urlBase) =>
        new LinkDTO[]
        {
            new LinkDTO($"{urlBase}/{id}", "get_policy", "GET"),
            new LinkDTO($"{urlBase}", "update_policy", "PATCH"),
            new LinkDTO($"{urlBase.Replace("/apolice","")}/parcela/{{int:id}}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
        };

    public LinkDTO[] CreateLinksForGetAllPolicies(string urlBase) =>
        new LinkDTO[]
        {
            new LinkDTO($"{urlBase}/{{int:id}}", "get_policy", "GET"),
            new LinkDTO($"{urlBase}", "update_policy", "PATCH"),
            new LinkDTO($"{urlBase.Replace("/apolice","")}/parcela/{{int:id}}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
        };

    public LinkDTO[] CreateLinksForGetPolicyById(string urlBase) =>
        new LinkDTO[]
        {
            new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
            new LinkDTO($"{urlBase}", "update_policy", "PATCH"),
            new LinkDTO($"{urlBase.Replace("/apolice","")}/parcela/{{int:id}}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
        };

    public LinkDTO[] CreateLinksForUpdatedPolicy(long id, string urlBase) =>
        new LinkDTO[]
        {
            new LinkDTO($"{urlBase}/{id}", "get_policy", "GET"),
            new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
            new LinkDTO($"{urlBase.Replace("/apolice","")}/parcela/{{int:id}}/pagamento?datePagamento={DateTime.Now}", "register_payment", "POST")
        };

    public LinkDTO[] CreateLinksForRegisterPaymentForPolicy(string urlBase) =>
        new LinkDTO[]
        {
            new LinkDTO($"{urlBase}", "create_policy", "POST"),
            new LinkDTO($"{urlBase}/{{int:id}}", "get_policy", "GET"),
            new LinkDTO($"{urlBase}?skip=0&take=100", "get_all_policy", "GET"),
            new LinkDTO($"{urlBase}", "update_policy", "PATCH")
        };
}
