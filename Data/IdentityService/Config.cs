using IdentityServer4.Models;
using IdentityServer4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IdentityService
{
    public class Config
    {
        // định nghĩa các Resource gì  // cho user quản lý   // chả về 1 danh sách các identityresource
        public static IEnumerable<IdentityResource> GetIdentityResources =>
          new IdentityResource[]
          {
              // user quản lý nhứng gì này // chuẩn của identity ít nhất phải có 2 thằng này
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
              //vd:
              // new IdentityResources.Email(),
              // new IdentityResources.Phone()
          };

        // danh sách các Api ở đây ta chỉ có mỗi thằng knowledgespace
        public static IEnumerable<ApiResource> GetApiResources =>
            new ApiResource[]
            {
                new ApiResource("api1", "WebAppAPI")
            };



        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("api1", "WebAppAPI")
        };



        /*  định nghĩa ra các Client chín là các ứng dụng ta định làm , chính là webportal , server (chính là swagger) và .. */

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebApp",
                    ClientSecrets = { new Secret("secret".Sha256()) },//  mã hóa theo Sha256

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    AllowedCorsOrigins={"http://localhost/" },

                    // đăng nhập thành công thì redirect lại theo đường dẫn này
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // khi logout nó chạy cổng này và sử lý logout bên kia
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    // ở client này cho phép chuy cập đến những cái này
                    AllowedScopes = new List<string>
                    {
                        // ở đây chúng ta cho chuy cập cả thông tin user lần api
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    }
                 },
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "Swagger Client",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { "https://localhost:7217/swagger/oauth2-redirect.html" }, // chuyển hướng
                    PostLogoutRedirectUris = { "https://localhost:7217/swagger/oauth2-redirect.html" },// chuyển hướng đăng xuất
                    AllowedCorsOrigins =     { "https://localhost:7217" }, // cho phép nguồn gốc cores

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },

            };
    }
}
