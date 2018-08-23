﻿

namespace Sales.Services
{

    
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Helpers;

    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = Languages.TurnOnInternet,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = Languages.NoInternet,
                };
            }

            return new Response
            {
                IsSucces = true,
            };
        }
        public async Task<Response> Getlist<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucces = false,
                        Message = answer,
                   };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSucces = true,
                    Result = list,


                };
                
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = ex.Message,

                };
            }
        }
        
    }
}
