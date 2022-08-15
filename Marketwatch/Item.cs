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
        public bool hasStickers { get; internal set; }
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
                    // Doppler Skins
                    // Talon-Knive, Stiletto-Knive, Navaja-Knife, Ursus-Knife, Butterfly-Knife, Huntsman-Knife, Falchion-Knife, Bowie-Knife,
                    // Shadow-Daggers, Karambit, M9-Bayonet, Bayonet, Flip-Knife, Gut-Knife

                    // Gamma Doppler
                    // Butterfly-Knife, Bowie-Knife, Huntsman-Knife, Falchion-Knife, Shadow-Daggers, Karambit, M9-Bayonet, Flip-Knife, Gut-Knife
                    // Glock-18

                    // Ruby
                    case 415:    // Default - Doppler
                        pattern = "Ruby";
                        break;

                    // Sapphire
                    case  416:   // Default - Doppler
                    case  619:   // Butterfly-Knife - Doppler
                                 // Shadow-Daggers - Doppler
                        pattern = "Sapphire";
                        break;

                    case  417:   // Default - Doppler
                    case  617:   // Butterfly-Knife - Doppler
                                 // Shadow-Daggers - Doppler
                        pattern = "Black Pearl";
                        break;

                    // Emerald
                    case  568:   // Default - Gamma Doppler
                    case 1119:   // Glock-18 - Gamma Doppler
                        pattern = "Emerald";
                        break;

                    // Phase 1
                    case  418:   // Default - Doppler
                    case  569:   // Default - Gamma Doppler
                    case  852:   // Talon-Knife - Doppler
                    case 1120:   // Glock-18 - Gamma Doppler
                        pattern = "Phase 1";
                        break;

                    // Phase 2
                    case  419:   // Default - Doppler
                    case  570:   // Default - Gamma Doppler
                    case  618:   // Butterfly-Knife - Doppler
                                 // Shadow-Daggers - Doppler
                    case  853:   // Talon-Knife - Doppler
                    case 1121:   // Glock-18 - Gamma Doppler
                        pattern = "Phase 2";
                        break;

                    // Phase 3
                    case  420:   // Default - Doppler
                    case  571:   // Default - Gamma Doppler
                    case  854:   // Talon-Knife - Doppler
                    case 1122:   // Glock-18 - Gamma Doppler
                        pattern = "Phase 3";
                        break;

                    // Phase 4
                    case  421:   // Default - Doppler
                    case  572:   // Default - Gamma Doppler
                    case  855:   // Talon-Knife - Doppler
                    case 1123:   // Glock-18 - Gamma Doppler
                        pattern = "Phase 4";
                        break;

                    // All Other Skins
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
            item.hasStickers = false;

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

            if (this.hasStickers)
                nameCell.Value = nameCell.Value + " ( * )";

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
