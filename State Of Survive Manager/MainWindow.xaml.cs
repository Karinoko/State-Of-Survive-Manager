using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace State_Of_Survive_Manager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool AutoMatching = false;

        public MainWindow()
        {
            InitializeComponent();
            ResetSeparate();
        }

        private void btn_Simulate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = 0;
                int b = 0;

                if (lbl_RaidDamage_Min.Text != "0" && lbl_RaidDamage_Max.Text != "0")
                {
                    a = GetAverange(int.Parse(lbl_RaidDamage_Min.Text.Replace(" ", "")), int.Parse(lbl_RaidDamage_Max.Text.Replace(" ", "")));
                    lbl_RaidDamage.Text = a.ToString();
                    ResetSeparate();
                }
                if (lbl_AmountUnits_Min.Text != "0" && lbl_AmountUnits_Max.Text != "0")
                {
                    b = GetAverange(int.Parse(lbl_AmountUnits_Min.Text.Replace(" ", "")), int.Parse(lbl_AmountUnits_Max.Text.Replace(" ", "")));
                    lbl_AmountUnits.Text = b.ToString();
                    ResetSeparate();
                }
                RaidOperator raidOperator = new RaidOperator(
                    int.Parse(lbl_BossHP.Text.Replace(" ", "")),
                    int.Parse(lbl_RaidCapacity.Text.Replace(" ", "")),
                    int.Parse(lbl_RaidDamage.Text.Replace(" ", "")),
                    int.Parse(lbl_AmountPlayers.Text.Replace(" ", "")),
                    int.Parse(lbl_AmountUnits.Text.Replace(" ", "")),
                    (bool)cbx_RaidParallel.IsChecked,
                    (bool)cbx_ToUsers.IsChecked,
                    (bool)cbx_AutoMatching.IsChecked);

                sum_PossibleRaids.Content = raidOperator.GetPossibleRaids();
                sum_MaxPlayes.Content = raidOperator.GetMaxPlayes();
                sum_FreeSlots.Content = raidOperator.GetFreeSlots();
                sum_AmountUnits.Content = raidOperator.GetAmountUnits();
                sum_AmountRaids.Content = raidOperator.GetAmountRaids();
                sum_AmountPlayers.Content = raidOperator.GetAmountPlayers();
                sum_AmountEmergency.Content = raidOperator.GetAmountEmergency();
                sum_PossibleEmergency.Content = raidOperator.GetPossibleEmergency();
            }
            catch (Exception)
            {

                return;
            }
        }

        private int GetAverange(int a, int b)
        {
            return (int)Math.Ceiling((a + b) / 2.0);
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            lbl_BossHP.Text = "0";
            lbl_RaidCapacity.Text = "0";
            lbl_RaidDamage.Text = "0";
            lbl_AmountPlayers.Text = "0";
            lbl_AmountUnits.Text = "0";

            sum_PossibleRaids.Content = "0";
            sum_MaxPlayes.Content = "0";
            sum_FreeSlots.Content = "0";
            sum_AmountUnits.Content = "0";
            sum_AmountRaids.Content = "0";
            sum_AmountPlayers.Content = "0";
            sum_AmountEmergency.Content = "0";
            sum_PossibleEmergency.Content = "0";
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Record record = new Record()
            {
                BossHp = lbl_BossHP.Text.Replace(" ",""),
                RaidCapacity = lbl_RaidCapacity.Text.Replace(" ", ""),
                RaidDamage = lbl_RaidDamage.Text.Replace(" ", ""),
                RaidDamageMin = lbl_RaidDamage_Min.Text.Replace(" ", ""),
                RaidDamageMax = lbl_RaidDamage_Max.Text.Replace(" ", ""),
                AmountPlayers = lbl_AmountPlayers.Text.Replace(" ", ""),
                AmountUnits = lbl_AmountUnits.Text.Replace(" ", ""),
                AmountUnitsMin = lbl_AmountUnits_Min.Text.Replace(" ", ""),
                AmountUnitsMax = lbl_AmountUnits_Max.Text.Replace(" ", ""),
                IsParallel = (bool)cbx_RaidParallel.IsChecked
            };

            ConfigReader.SaveConfiguration(record);
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Record record = ConfigReader.ReadConfiguration();

                lbl_BossHP.Text = record.BossHp;
                lbl_RaidCapacity.Text = record.RaidCapacity;
                lbl_RaidDamage.Text = record.RaidDamage;
                lbl_RaidDamage_Min.Text = record.RaidDamageMin;
                lbl_RaidDamage_Max.Text = record.RaidDamageMax;
                lbl_AmountPlayers.Text = record.AmountPlayers;
                lbl_AmountUnits.Text = record.AmountUnits;
                lbl_AmountUnits_Min.Text = record.AmountUnitsMin;
                lbl_AmountUnits_Max.Text = record.AmountUnitsMax;
                cbx_RaidParallel.IsChecked = record.IsParallel;

                ResetSeparate();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void lbl__LostFocus(object sender, RoutedEventArgs e)
        {
            SeparateDecimals(sender);
            RestrictedTextBox(sender);
        }

        private void ResetSeparate()
        {
            SeparateDecimals(lbl_BossHP);
            SeparateDecimals(lbl_RaidCapacity);
            SeparateDecimals(lbl_RaidDamage);
            SeparateDecimals(lbl_AmountPlayers);
            SeparateDecimals(lbl_AmountUnits);
        }

        private void RestrictedTextBox(object sender)
        {
            TextBox textBox = sender as TextBox;

            var tmp = textBox.Text.Replace(" ", "");

            if (tmp.Length > 1 && tmp[tmp.Length - 1] == '0')
                tmp = tmp.Remove(0, 1);
            for (int i = 0; i < tmp.Length; i++)
            {
                var ss = tmp[i];
                char[] sss = tmp.ToCharArray();
                if(ss != '0' | ss != '1' | ss != '2' | ss != '3' | ss != '4' | ss != '5' | ss != '6' | ss != '7' | ss != '8' | ss != '9')
                    tmp = tmp.Remove(i, 1);
            }
        }

        private void SeparateDecimals(object sender)
        {
            TextBox textBox = sender as TextBox;

            var tmp = textBox.Text;

            tmp = tmp.Replace(" ", "");

            if (tmp.Replace(" ", "").Length > 3)
                tmp = tmp.Insert(tmp.Length - 3, " ");
            if (tmp.Replace(" ","").Length > 6)
                tmp = tmp.Insert(tmp.Length - 7, " ");
            if (tmp.Replace(" ", "").Length > 9)
                tmp = tmp.Insert(tmp.Length - 11, " ");
            if (tmp.Replace(" ", "").Length > 12)
                tmp = tmp.Insert(tmp.Length - 15, " ");

            textBox.Text = tmp;
        }

        private void cbx_AutoMatching_Checked(object sender, RoutedEventArgs e)
        {
            AutoMatching = (bool)cbx_AutoMatching.IsChecked;
            cbx_ToUsers.IsEnabled = true;
            lbl_AmountPlayers.IsEnabled = false;
            lbl_AmountUnits.IsEnabled = false;
            lbl_RaidDamage.IsEnabled = false;
        }

        private void cbx_AutoMatching_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoMatching = (bool)cbx_AutoMatching.IsChecked;
            cbx_ToUsers.IsEnabled = false;
            lbl_AmountPlayers.IsEnabled = true;
            lbl_AmountUnits.IsEnabled = true;
            lbl_RaidDamage.IsEnabled = true;
        }

        private void cbx_MidT6_Checked(object sender, RoutedEventArgs e)
        {
            if (cbx_MidT4 != null && cbx_MidT9 != null)
            {
                cbx_MidT9.IsChecked = false;
                cbx_MidT4.IsChecked = false;
            }
        }

        private void cbx_MidT4_Checked(object sender, RoutedEventArgs e)
        {
            if (cbx_MidT6 != null && cbx_MidT9 != null)
            {
                cbx_MidT9.IsChecked = false;
                cbx_MidT6.IsChecked = false;
            }
        }

        private void cbx_MidT9_Checked(object sender, RoutedEventArgs e)
        {
            if (cbx_MidT4 != null && cbx_MidT6 != null)
            {
                cbx_MidT6.IsChecked = false;
                cbx_MidT4.IsChecked = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();
        }
    }
}
