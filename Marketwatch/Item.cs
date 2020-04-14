using SteamKit2;
using SteamKit2.GC;
using SteamKit2.GC.CSGO.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Marketwatch {
    public class Item {
        private uint _paintIndex;

        public UInt64 param_s { get; set; }
        public UInt64 param_a { get; set; }
        public UInt64 param_d { get; set; }
        public UInt64 param_m { get; set; }

        public Task<Image> ownerImage { get; set; }
        public Task<Image> previewImage { get; set; }
        public bool hasNameTag { get; internal set; }
        public double floatValue { get; set; }
        public string pattern { get; private set; }
        public string price { get; set; }
        public string convertedFee { get; set; }
        public string convertedPrice { get; set; }
        public string convertedTotal { get; set; }
        public string name { get; set; }
        public string previewHighResolutionImageURL { get; set; }
        public string[] stickerNames { get; set; }
        public List<Task<Image>> stickerImages { get; set; } = new List<Task<Image>>();

        public uint paintIndex {
            get { return _paintIndex; }
            set {
                switch (value) {
                    case 415:
                        pattern = "Ruby";
                        break;
                    case 416:
                        pattern = "Sapphire";
                        break;
                    case 417:
                        pattern = "Black Pearl";
                        break;
                    case 568:
                        pattern = "Emerald";
                        break;
                    case 418:
                    case 569:
                        pattern = "Phase 1";
                        break;
                    case 419:
                    case 570:
                        pattern = "Phase 2";
                        break;
                    case 420:
                    case 571:
                        pattern = "Phase 3";
                        break;
                    case 572:
                    case 421:
                        pattern = "Phase 4";
                        break;
                    default:
                        pattern = null;
                        break;
                }
                _paintIndex = value;
            }
        }

      public static Item newItemFromURL(String url) {
            Item item = new Item();

            //set default to false
            item.hasNameTag = false;

            Match match = Regex.Match(url, "(?:S(?<param_s>[0-9]+)|M(?<param_m>[0-9]+))A(?<param_a>[0-9]+)D(?<param_d>[0-9]+)");

            item.param_s = match.Groups["param_s"].Success ? UInt64.Parse(match.Groups["param_s"].Value) : 0;
            item.param_a = UInt64.Parse(match.Groups["param_a"].Value);
            item.param_d = UInt64.Parse(match.Groups["param_d"].Value);
            item.param_m = match.Groups["param_m"].Success ? UInt64.Parse(match.Groups["param_m"].Value) : 0;

            return item;
        }

        public bool requestItemPreviewData(SteamGameCoordinator steamGameCoordinator) {
            var requestPreviewData = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockRequest>((uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockRequest);

            requestPreviewData.Body.param_s = this.param_s;
            requestPreviewData.Body.param_a = this.param_a;
            requestPreviewData.Body.param_d = this.param_d;
            requestPreviewData.Body.param_m = this.param_m;

            steamGameCoordinator.Send(requestPreviewData, 730);

            return true;
        }

        public async Task<DataGridViewRow> itemToRow(DataGridView dataGridViewAsTemplate) {
            DataGridViewRow row = CloneWithValues(dataGridViewAsTemplate.RowTemplate);
            DataGridViewRichTextBoxCell nameCell = (DataGridViewRichTextBoxCell)row.Cells[dataGridViewAsTemplate.Columns["itemNameColumn"].Index];

            if (!String.IsNullOrEmpty(this.pattern))
                nameCell.Value = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1031{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif; } }\viewkind4\uc1\pard\f0\fs17\up0 " + this.name.Replace("★", @"\u9733?") + @" \up6\fs13 " + this.pattern + @"\up0\par}";
            else
                nameCell.Value = this.name;

            //row.Cells[dataGridViewAsTemplate.Columns["itemNameColumn"].Index] = nameCell;

            row.Cells[dataGridViewAsTemplate.Columns["priceColumn"].Index].Value = this.price;
            row.Cells[dataGridViewAsTemplate.Columns["floatColumn"].Index].Value = this.floatValue;
            row.Cells[dataGridViewAsTemplate.Columns["inspectLinkColumn"].Index].Value = "steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M" + this.param_m + "A" + this.param_a + "D" + this.param_d; //inspect link
            row.Cells[dataGridViewAsTemplate.Columns["buyLinkColumn"].Index].Value = "javascript:BuyMarketListing('listing', '" + this.param_m + "', 730, '2', '" + this.param_a + "')";
            row.Cells[dataGridViewAsTemplate.Columns["accImageColumn"].Index].Value = await ownerImage;
            row.Cells[dataGridViewAsTemplate.Columns["itemPreviewColumn"].Index].Value = await previewImage;

            return row;
        }

        public DataGridViewRow CloneWithValues(DataGridViewRow row) {
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++) {
                clonedRow.Cells[index].Value = row.Cells[index].Value;
            }
            return clonedRow;
        }

        public override string ToString() {
            return $"S[{param_s}] A[{param_a,10}] D[{param_d,20}] M[{param_m,18}]";
        }
    }
}
