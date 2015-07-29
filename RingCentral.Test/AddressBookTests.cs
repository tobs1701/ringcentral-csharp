﻿using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class AddressBookTests : TestConfiguration
    {
        private const string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";




        [TestFixtureSetUp]
        public void Setup()
        {
            mockResponseHandler.AddGetMockResponse(
                    new Uri(ApiEndPoint + AddressBookEndPoint + "/1"),
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/123123\"," +
                        "\"availability\": \"Alive\"," + "\"id\": 123123 ," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," +
                        "\"businessAddress\": { " + "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\"," + "\"zip\": \"94123\" } }")

                    });
            mockResponseHandler.AddGetMockResponse(
                new Uri(ApiEndPoint + AddressBookEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact?sortBy=FirstName\"," +
                     "\"records\": [ " + "{" + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/1\"," +
                         "\"availability\": \"Alive\"," + "\"id\": 1," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," + "\"businessAddress\": { " +
                           "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\", " + "\"zip\": \"94123\" " + "}" + "}," + "{" +
                         "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/2/extension/2/address-book/contact/2\"," + "\"availability\": \"Alive\"," +
                         "\"id\": 2," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                             "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"" + "}" + "}" +
                       "], \"paging\" : { \"page\": 1, \"totalPages\": 1, \"perPage\": 100, \"totalElements\": 2, \"pageStart\": 0, \"pageEnd\": 1 }, " +
                       "\"navigation\": {  \"firstPage\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/contact?sortBy=FirstName&page=1&perPage=100\" },  " +
                       "\"lastPage\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/contact?sortBy=FirstName&page=1&perPage=100\" } }," +
                       "\"groups\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/group\" } }"

                       )
                });
            mockResponseHandler.AddDeleteMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint + "/3"),
                   new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"message\": \"Deleted\" }") });
            mockResponseHandler.AddPostMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/3\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 3 ," + "\"firstName\": \"Jim\"," + "\"lastName\": \"Johns\"," + "\"businessAddress\": { " + "\"street\": \"5 Marina Blvd\", " + "\"city\": \"San-Francisco\"," +
                       "\"state\": \"CA\"," + "\"zip\": \"94123\" } }")
                   });
            mockResponseHandler.AddPutMockResponse(
                new Uri(ApiEndPoint + AddressBookEndPoint + "/5"),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/5\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 5 ," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                             "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"}" + "} ")
                   });




        }

        [Test]
        public void GetContactFromAddressBook()
        {
            string result = Platform.GetRequest(AddressBookEndPoint + "/1");
            JToken token = JObject.Parse(result);
            var firstNameResponse = (string)token.SelectToken("firstName");

            Assert.AreEqual(firstNameResponse, "Delete");
        }
        [Test]
        public void GetAddressBook()
        {
            string result = Platform.GetRequest(AddressBookEndPoint);
            JToken token = JObject.Parse(result);
            var firstName = (string)token.SelectToken("records")[0].SelectToken("firstName");

            Assert.AreEqual("Delete", firstName);

            firstName = (string)token.SelectToken("records")[1].SelectToken("firstName");

            Assert.AreEqual("Vanessa", firstName);

        }
        [Test]
        public void DeleteContactFromAddressBook()
        {
            string result = Platform.DeleteRequest(AddressBookEndPoint + "/3");
            JToken token = JObject.Parse(result);
            var message = (string)token.SelectToken("message");
            Assert.AreEqual("Deleted", message);
        }
        [Test]
        public void CreateAddressBookContact()
        {
            string jsonData = "{\"firstName\": \"Vanessa\", " +
                             "\"lastName\": \"May\", " +
                             "\"businessAddress\": " +
                             "{ " +
                             "\"street\": \"2 Marina Blvd\", " +
                             "\"city\": \"San-Francisco\", " +
                             "\"state\": \"CA\", " +
                             "\"zip\": \"94123\"}" +
                             "}";
            Platform.SetJsonData(jsonData);
            string result = Platform.PostRequest(AddressBookEndPoint);
            JToken token = JObject.Parse(result);

            var firstName = (string)token.SelectToken("firstName");
            Assert.AreEqual("Jim", firstName);


        }
        [Test]
        public void UpdateContactInAddressbook()
        {
            string jsonData = "{\"firstName\": \"Vanessa\", " +
                              "\"lastName\": \"May\", " +
                              "\"businessAddress\": " +
                              "{ " +
                              "\"street\": \"3 Marina Blvd\", " +
                              "\"city\": \"San-Francisco\", " +
                              "\"state\": \"CA\", " +
                              "\"zip\": \"94123\"}" +
                              "}";
            Platform.SetJsonData(jsonData);

            string result = Platform.PutRequest(AddressBookEndPoint + "/5");

            JToken token = JObject.Parse(result);
            var street = (string)token.SelectToken("businessAddress").SelectToken("street");

            Assert.AreEqual(street, "3 Marina Blvd");
        }
    }
}