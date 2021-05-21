namespace MicroService.Consul.Test {
    using global::Consul;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using System.Linq;

    public class UnitTest1 {
        [Fact]
        public async Task Test1() {

            using var client = new ConsulClient();
            {
                var queryResult = await client.Health.Service("OrderService", string.Empty, true);
                var nodes = await client.Catalog.Nodes();
                foreach (var serviceEntry in queryResult.Response) {


                    var serviceNode = nodes.Response.FirstOrDefault(n => n.Address == serviceEntry.Service.Address);

                }

            }
            {
            //    var putPair = new KVPair("hello")
            //{
            //        Value = Encoding.UTF8.GetBytes("Hello Consul")
            //    };

            //    var putAttempt = await client.KV.Put(putPair);

            //    if (putAttempt.Response) {
            //        var getPair = await client.KV.Get("hello");
            //        Assert.Equal("Hello Consul", Encoding.UTF8.GetString(getPair.Response.Value, 0,
            //            getPair.Response.Value.Length));
            //    }
            }
           

        }
    }
}
