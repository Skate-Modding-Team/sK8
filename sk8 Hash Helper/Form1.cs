using System.Text;
using sK8.Andale;

namespace sk8_Hash_Helper
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void HashSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            switch (HashSelector.SelectedIndex)
            {
                case 0: //RW HASH 64
                    ulong rwhash64 = sK8.Renderware.RwHash.RwHash64String(InputBox.Text);
                    OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(rwhash64).Reverse().ToArray()).Replace("-", "");
                    break;

                case 1: //RW HASH 32
                    uint rwhash32 = sK8.Renderware.RwHash.RwHash32String(InputBox.Text);
                    OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(rwhash32).Reverse().ToArray()).Replace("-", "");
                    break;

                case 2: //LOOKUP 8
                    ulong lookup8 = sK8.Attribulator.Lookup8.Hash(InputBox.Text);
                    OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(lookup8).Reverse().ToArray()).Replace("-", "");
                    break;

                case 3: // DJB2 64
                    ulong djb264 = sK8.DJB2Hash.Hash64(InputBox.Text);
                    OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(djb264).Reverse().ToArray()).Replace("-", "");
                    break;

                case 4: // DJB2 32
                    uint djb232 = sK8.DJB2Hash.Hash32(InputBox.Text);
                    OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(djb232).Reverse().ToArray()).Replace("-", "");
                    break;

                case 5: // RW HASH 32 BUFFER

                    try
                    {
                        uint rwhash32b = sK8.Renderware.RwHash.RwHash32Buffer(Convert.FromHexString(InputBox.Text.ToUpper().Replace(" ", "").Replace("-", "")));
                        OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(rwhash32b).Reverse().ToArray()).Replace("-", "");
                    }
                    catch
                    {
                        OutputBox.Text = "Invalid Byte Array.";
                    }

                    break;
                
                case 6: // RW HASH 64 BUFFER

                    try
                    {
                        ulong rwhash64b = sK8.Renderware.RwHash.RwHash64Buffer(Convert.FromHexString(InputBox.Text.ToUpper().Replace(" ", "").Replace("-", "")));
                        OutputBox.Text = BitConverter.ToString(BitConverter.GetBytes(rwhash64b).Reverse().ToArray()).Replace("-", "");
                    }
                    catch
                    {
                        OutputBox.Text = "Invalid Byte Array.";
                    }

                    break;

                case 7: // FAST STRING

                    try
                    {
                        byte[] fs32 = FastString.Encode(InputBox.Text);
                        OutputBox.Text = BitConverter.ToString(fs32).Replace("-", "");
                    }
                    catch (Exception e)
                    {
                        OutputBox.Text = e.Message;
                    }

                    break;

                case 8: // INVERSE FAST STRING

                    try
                    {
                        string decoded = InputBox.Text;
                        byte[] decodedHex = Convert.FromHexString(decoded.ToUpper().Replace(" ", "").Replace("-","")).ToArray();
                        if (decodedHex.Length % 4 != 0)
                            throw new Exception("Bytes are not a multiple of 4!");
                        decoded = FastString.Decode(decodedHex);
                        OutputBox.Text = decoded;
                    }
                    catch (Exception e)
                    {
                        OutputBox.Text = e.Message;
                    }
                    break;
            }
        }
    }
}
