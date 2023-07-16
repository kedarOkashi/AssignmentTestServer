using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";

            while (input != "6")
            {
                Console.WriteLine("Please select an action: \n1.\tAdding a new user to the database \n2.\tPrint all users. \n3.\tRetrieving a user by ID \n4.\tUpdating user information  \n5.\tDeleting a user \n6.\t Exit");
                Console.Write("\tChoosen Option :");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        _ = CreateUserAsync();
                        break;
                    case "2":
                        _ = GetUsersAsync();
                        break;
                    case "3":
                        _ = RetrievingUserByIdAsync();
                        break;
                    case "4":
                        _ = UpdateUserAsync();
                        break;
                    case "5":
                        _ = DeleteUserByIdAsync();
                        break;
                    case "6":

                        break;
                    default:
                        Console.WriteLine("Wrong Input!");
                        break;
                }
                Console.WriteLine("Press any key to go back to main menu");
                Console.ReadKey();
                Console.Clear();
            }
        }
        static async Task CreateUserAsync()
        {
            string name, email, password;
            Console.Write("please enter name for user: ");
            name = Console.ReadLine();
            Console.Write("please enter email for user: ");
            email = Console.ReadLine();
            Console.Write("please enter password for user: ");
            password = Console.ReadLine();

            using (HttpClient client = new HttpClient())
            {

                string jsonBody = "{\"Name\":\"" + name + "\",\"Email\":\"" + email + "\",\"Password\":\"" + password + "\"}";
                HttpContent content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                // Create HttpRequestMessage
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/CreateUser");
                request.Content = content;

                try
                {
                    // Send request
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Request was successful
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                    }
                    else
                    {
                        // Request failed
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    // Exception occurred
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        static async Task GetUsersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                // Send GET request
                HttpResponseMessage response = await client.GetAsync("http://localhost:8080/GetUsers");

                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    // Request was successful
                    string responseContent = "List of users.\r\n";
                    responseContent += await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseContent);
                }
                else
                {
                    // Request failed
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                }
            }
        }

        static async Task RetrievingUserByIdAsync()
        {
            string responseContent;
            Console.Write("Please enter user id: ");
            string id = Console.ReadLine();
            using (HttpClient client = new HttpClient())
            {
                // Send GET request
                HttpResponseMessage response = await client.GetAsync($"http://localhost:8080/GetUser?id={id}");

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    responseContent = $"user with id:{id} not exists";
                    Console.WriteLine(responseContent);
                }
                else
                {
                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Request was successful

                        responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);

                    }
                    else
                    {
                        // Request failed
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }              
            }
        }

        static async Task UpdateUserAsync()
        {
            string id, name, email, password;
            Console.Write("Please enter the user ID number you want to update: ");
             id = Console.ReadLine();

            using (HttpClient client = new HttpClient())
            {
                // Send GET request
                HttpResponseMessage response = await client.GetAsync($"http://localhost:8080/GetUser?id={id}");
                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    // Request was successful
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        Console.WriteLine($"Sorry user with id:{id} not exist");
                    }
                    else
                    {
                        Console.Write("please enter name for user: ");
                        name = Console.ReadLine();
                        Console.Write("please enter email for user: ");
                        email = Console.ReadLine();
                        
                        Console.Write("please enter password for user: ");
                        password = Console.ReadLine();

                        using (HttpClient client2 = new HttpClient())
                        {
                            // Create JSON content
                            string jsonBody = "{\"Id\":\"" + id + "\",\"Name\":\"" + name + "\",\"Email\":\"" + email + "\",\"Password\":\"" + password + "\"}";
                            HttpContent content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                            // Send PUT request
                            HttpResponseMessage response2 = await client2.PutAsync("http://localhost:8080/UpdateUser", content);

                            // Handle response
                            if (response.IsSuccessStatusCode)
                            {
                                // Request was successful
                                string responseContent2 = await response2.Content.ReadAsStringAsync();
                                Console.WriteLine("Response: " + responseContent2);
                            }
                            else
                            {
                                // Request failed
                                Console.WriteLine("Request failed with status code: " + response.StatusCode);
                            }
                        }

                    }
                }
                else
                {
                    // Request failed
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                }
            }

        }

        static async Task DeleteUserByIdAsync()
        {
            string id,responseContent;
            Console.Write("Please enter the user ID number you want to Delete: ");
            id = Console.ReadLine();

            using (HttpClient client = new HttpClient())
            {
                // Send DELETE request
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:8080/DeleteUser?id={id}");

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Sorry, user with id:{id} not exists");
                    return;

                }              
                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    // Request was successful
                     responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);

                }
                else
                {
                    // Request failed
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                }
            }
        }

    }
}
