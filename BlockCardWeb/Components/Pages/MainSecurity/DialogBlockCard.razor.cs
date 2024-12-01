using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Npgsql;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class DialogBlockCard
    {
        [CascadingParameter] public MudDialogInstance Dialog { get; set; }
        [Parameter] public string messagecontent { get; set; }
        [Parameter] public QueryVoucherResponse queryvouchermodel { get; set; }

        public UserSqlService userSqlService = new UserSqlService();
        public List<Supplier> suppliermodel = new List<Supplier>();
        public List<Province> provincemodel = new List<Province>();

        protected override async Task OnInitializedAsync()
        {
            await Task.WhenAll(getSupplier() , getProvince());
            if (queryvouchermodel != null)
            {

            }


        }


        public async Task getSupplier()
        {
            var sql = "select * from uvc_supplier_card";
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                List<Supplier> modes = new List<Supplier>();
                Console.WriteLine(modes);
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modes.Add(new Supplier { supplier_id = Convert.ToInt32(reader["supplier_id"]), supplier_name = reader["supplier_name"].ToString() });
                    }
                }
                if (modes.Count > 0)
                {
                    suppliermodel = modes;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex != null)
                {
                    Console.WriteLine(ex);
                }
            }

        }
        public async Task getProvince()
        {
            var sql = "select * from uvc_province";
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                List<Province> modes = new List<Province>();
                Console.WriteLine(modes);
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modes.Add(new Province { provinceid = Convert.ToInt32(reader["id"]), provincename = reader["province"].ToString() });
                    }
                }
                if (modes.Count > 0)
                {
                    provincemodel = modes;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex != null)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        public async Task Close()
        {
            Dialog.Close();
        }

    }
}
