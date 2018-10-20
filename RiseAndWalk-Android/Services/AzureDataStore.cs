using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RiseAndWalk_Android.Services
{
    //TODO: DELETE THIS SHIT
    public class AzureDataStore : IDataStore<Alarm>
    {
        //HttpClient client;
        //IEnumerable<Item> items;

        //public AzureDataStore()
        //{
        //    client = new HttpClient();
        //    client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

        //    items = new List<Item>();
        //}

        //public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        //{
        //    if (forceRefresh)
        //    {
        //        var json = await client.GetStringAsync($"api/item");
        //        items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Item>>(json));
        //    }

        //    return items;
        //}

        //public async Task<Item> GetItemAsync(string id)
        //{
        //    if (id != null)
        //    {
        //        var json = await client.GetStringAsync($"api/item/{id}");
        //        return await Task.Run(() => JsonConvert.DeserializeObject<Item>(json));
        //    }

        //    return null;
        //}

        //public async Task<bool> AddItemAsync(Item item)
        //{
        //    if (item == null)
        //        return false;

        //    var serializedItem = JsonConvert.SerializeObject(item);

        //    var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

        //    return response.IsSuccessStatusCode;
        //}

        //public async Task<bool> UpdateItemAsync(Item item)
        //{
        //    if (item == null || item.Id == null)
        //        return false;

        //    var serializedItem = JsonConvert.SerializeObject(item);
        //    var buffer = Encoding.UTF8.GetBytes(serializedItem);
        //    var byteContent = new ByteArrayContent(buffer);

        //    var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

        //    return response.IsSuccessStatusCode;
        //}

        //public async Task<bool> DeleteItemAsync(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return false;

        //    var response = await client.DeleteAsync($"api/item/{id}");

        //    return response.IsSuccessStatusCode;
        //}

        public Task<bool> AddItemAsync(Alarm item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Alarm item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Alarm> GetItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Alarm>> IDataStore<Alarm>.GetItemsAsync(bool forceRefresh)
        {
            throw new NotImplementedException();
        }
    }
}