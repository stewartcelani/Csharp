using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WasmLogToBrowser.Client;
using WasmLogToBrowser.Client.Library;
using WasmLogToBrowser.Client.Services;
using WasmLogToBrowser.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DebugService>();
builder.Services.AddScoped<JsConsole>();

await builder.Build().RunAsync();
