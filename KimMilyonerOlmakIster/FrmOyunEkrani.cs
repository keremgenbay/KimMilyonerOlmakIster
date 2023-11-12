using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Media;

namespace KimMilyonerOlmakIster
{
    public partial class FrmOyunEkrani : Form

    {
        public bool a = false;
        private int soruSuresi = 30; // Saniye cinsinden soru süresi
        private Timer soruSuresiTimer = new Timer();

        private SoundPlayer player;
        public FrmOyunEkrani()
        {
            InitializeComponent();

            soruSuresiTimer.Interval = 1000; // Timer her 1 saniyede bir çalışacak
            soruSuresiTimer.Tick += SoruSuresiTimer_Tick;

           
        }

        private List<Soru> sorular;
        private List<Soru> secilenSorular;
        private Random random = new Random();
        public int soruSayac = 0;
        private List<Button> butonlar;

        private void FrmOyunEkrani_Load(object sender, EventArgs e)
        {
            Image myimage = new Bitmap("C:\\Users\\KEREM\\Downloads\\background.png");
            this.BackgroundImage = myimage;
            soruSuresiTimer.Start();

            // Başlangıçta süreyi göster
            lblSoruSuresi.Text = "Süre: " + soruSuresi.ToString() + " saniye";

            
            butonlar = new List<Button> { btnA, btnB, btnC, btnD };


            foreach (Button buton in butonlar)
            {
                buton.Click += Buton_Click; // Buton_Click adlı olay dinleyiciyi her bir butona ekler.
            }

            // JSON dosyasından soruları yükle
            string json = File.ReadAllText("json/veriler.json");
            sorular = JsonConvert.DeserializeObject<List<Soru>>(json);

            // Belirli zorluk seviyelerine sahip soruları seç
            secilenSorular = SoruSec(1, 4, 5)
                .Concat(SoruSec(6, 9, 5))
                .Concat(SoruSec(10, 15, 3))
                .ToList();

            // Rastgele bir soruyu göster
            GosterRastgeleSoru();

            

           

        }

        private void SoruSuresiTimer_Tick(object sender, EventArgs e)
        {
            if (soruSuresi > 0)
            {  
                if (a)
                {
                    lblSoruSuresi.Text = "Süre: " + soruSuresi.ToString() + " saniye";
                }
                else
                {
                    soruSuresi--; // Süreyi bir saniye azalt
                    lblSoruSuresi.Text = "Süre: " + soruSuresi.ToString() + " saniye";
                }
                
            }
            else
            {
                soruSuresiTimer.Stop(); // Timer'ı durdur

                // Süre dolduğunda ne yapmak istediğinizi burada belirleyebilirsiniz.
                MessageBox.Show("Süre doldu!");
                FrmMenu frmMenu = new FrmMenu();
                frmMenu.ShowDialog();
                this.Hide();
            }
        }


        private void Buton_Click(object sender, EventArgs e)
        {
            Button tiklananButon = (Button)sender;

            foreach (Button buton in butonlar)
            {
                buton.Enabled = false;
            }
            timer1.Stop();
            soruSuresiTimer.Stop();
            string secilenCevap = tiklananButon.Text.Substring(0, 1); // Butonun metninden seçilen cevabı al

            Soru rastgeleSoru = secilenSorular[soruSayac - 1]; // soruSayac'ı 1 azaltarak mevcut soruyu al

            if (secilenCevap == rastgeleSoru.dogru_secenek)
            {
                tiklananButon.BackColor = Color.Green; // Doğru cevap ise yeşil renk
                string musicFilePath2 = "sound/paraefekt.wav";

                // SoundPlayer öğesini başlat ve müziği yükle
                player = new SoundPlayer(musicFilePath2);
                player.Play(); // Müziği sürekli çalmak için

                MessageBox.Show("Sıradaki soruya geçiniz!");
            }
            else
            {
                tiklananButon.BackColor = Color.Red;
                
                // Yanlış cevap ise kırmızı renk
                foreach (Button buton in butonlar)
                {
                    if (buton.Text.StartsWith(rastgeleSoru.dogru_secenek))
                    {
                        buton.BackColor = Color.Green;
                        break;
                    }
                }
                MessageBox.Show("2500 TL Teselli Ödülü Kazandınız!");
                FrmMenu frmMenu = new FrmMenu();
                frmMenu.Show();
                this.Close();
            }

            // Butonların tıklanabilirliğini tekrar etkinleştir
            foreach (Button buton in butonlar)
            {
                buton.Enabled = true;
            }
            a = false;
            // Sonraki soruya geç
            GosterRastgeleSoru();
        }

        private List<Soru> SoruSec(int baslangic, int bitis, int soruAdedi)
        {
            List<Soru> secilenler = sorular
                .Where(s => Convert.ToInt32(s.zorluk_seviyesi) >= baslangic && Convert.ToInt32(s.zorluk_seviyesi) <= bitis)
                .ToList();

            List<Soru> secilenSorular = new List<Soru>();

            if (secilenler.Count < soruAdedi)
            {
                MessageBox.Show("Belirtilen zorluk seviyelerine sahip yeterli soru bulunmuyor.");
            }
            else
            {
                while (secilenSorular.Count < soruAdedi)
                {
                    int index = random.Next(0, secilenler.Count);
                    secilenSorular.Add(secilenler[index]);
                    secilenler.RemoveAt(index);
                }
            }

            return secilenSorular;
        }

        private void GosterRastgeleSoru()
        {
            // Müzik dosyasının yolu
            string musicFilePath = "sound/muzik.wav";

            // SoundPlayer öğesini başlat ve müziği yükle
            player = new SoundPlayer(musicFilePath);
            player.PlayLooping(); // Müziği sürekli çalmak için

            soruSuresi = 30;
            soruSuresiTimer.Stop(); 
            soruSuresiTimer.Start();
            if (soruSayac < secilenSorular.Count)
            {
                // Her yeni soru için butonların arkaplan rengini temizle
                foreach (Button buton in butonlar)
                {
                    buton.BackColor = SystemColors.Control;
                }
                foreach (Button buton in butonlar)
                {
                    buton.Enabled = true;
                }

                Soru rastgeleSoru = secilenSorular[soruSayac];

                lblSoru.Text = (soruSayac + 1) + ".) " + rastgeleSoru.soru;

                string originalText = rastgeleSoru.soru;
                string newText = "";
                int charCount = 0;

                foreach (char character in originalText)
                {
                    if (charCount < 60) // 60 karakter sınırı
                    {
                        newText += character;
                        charCount++;
                    }
                    else
                    {
                        if (character == ' ') // Boşluk karakteri, kelime sonu kontrolü
                        {
                            newText += Environment.NewLine;
                            charCount = 0;
                        }
                        else
                        {
                            newText += character;
                        }
                    }
                }

                lblSoru.Text = (soruSayac + 1) + ".) " + newText;
                btnA.Text = "A) " + rastgeleSoru.secenekler[0];
                btnB.Text = "B) " + rastgeleSoru.secenekler[1];
                btnC.Text = "C) " + rastgeleSoru.secenekler[2];
                btnD.Text = "D) " + rastgeleSoru.secenekler[3];

                // Soru zorluk seviyesine göre ödül miktarını ayarla ve lblOdul'e yaz
                int odulMiktari = 0;
                switch (Convert.ToInt32(soruSayac +1))
                {
                    case 1:
                        odulMiktari = 0;
                        break;
                    case 2:
                        odulMiktari = 2500;
                        txtSoru2.BackColor = Color.Green;
                        break;
                    case 3:
                        odulMiktari = 5000;
                        txtSoru3.BackColor = Color.Green;
                        break;
                    case 4:
                        odulMiktari = 7500;
                        txtSoru4.BackColor = Color.Green;
                        break;
                    case 5:
                        odulMiktari = 15000;
                        txtSoru5.BackColor = Color.Green;
                        break;
                    case 6:
                        odulMiktari = 25000;
                        txtSoru6.BackColor = Color.Green;
                        break;
                    case 7:
                        odulMiktari = 50000;
                        txtSoru7.BackColor = Color.Green;
                        break;
                    case 8:
                        odulMiktari = 100000;
                        txtSoru8.BackColor = Color.Green;
                        break;
                    case 9:
                        odulMiktari = 200000;
                        txtSoru9.BackColor = Color.Green;
                        break;
                    case 10:
                        odulMiktari = 300000;
                        txtSoru10.BackColor = Color.Green;
                        break;
                    case 11:
                        odulMiktari = 500000;
                        txtSoru11.BackColor = Color.Green;
                        break;
                    case 12:
                        odulMiktari = 750000;
                        txtSorux.BackColor=Color.Green;
                        break;
                    case 13:
                        odulMiktari = 1000000;
                        txtSoru12.BackColor = Color.Green;
                        lblSoru.Text = "TEBRİKLER! ARTIK MİLYONERLER ARENASININ BİR ÜYESİSİNİZ!";
                        break;
                        // Diğer zorluk seviyeleri için aynı şekilde devam edebilirsiniz
                }

                lblOdul.Text = "Ödül: $" + odulMiktari;

                //lblDogruCevap.Tag = rastgeleSoru.dogru_secenek;
                //lblDogruCevap.Text = "";

                //rbSecenekA.Checked = false;
                //rbSecenekB.Checked = false;
                //rbSecenekC.Checked = false;
                //rbSecenekD.Checked = false;
                
                    soruSayac++;
                
            }
            else
            {
                MessageBox.Show("Tüm sorular tamamlandı!");
            }
            
            lblSoruSuresi.Text = "Süre: " + soruSuresi.ToString() + " saniye";

        }
       

        private void btnCevapla_Click(object sender, EventArgs e)
        {
            string secilenCevap = "";

            //if (rbSecenekA.Checked)
            //{
            //    secilenCevap = "A";
            //}
            //else if (rbSecenekB.Checked)
            //{
            //    secilenCevap = "B";
            //}
            //else if (rbSecenekC.Checked)
            //{
            //    secilenCevap = "C";
            //}
            //else if (rbSecenekD.Checked)
            //{
            //    secilenCevap = "D";
            //}

            //string dogruCevap = lblDogruCevap.Tag.ToString();

            //if (secilenCevap == dogruCevap)
            //{
            //    lblDogruCevap.Text = "Cevap doğru!";
            //}
            //else
            //{
            //    lblDogruCevap.Text = "Cevap yanlış. Doğru cevap: " + dogruCevap;
            //}
        }

        

        private void btnSonrakiSoru_Click_1(object sender, EventArgs e)
        {
            GosterRastgeleSoru();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(lblOdul.Text + " Kazandınız");
            soruSuresiTimer.Stop();
            soruSuresi = 30;
            FrmMenu frmMenu = new FrmMenu();
            frmMenu.Show();
            this.Close();
        }

        private void txtSoru2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnWhatsappArama_Click(object sender, EventArgs e)
        {
            string telefonNumarasi = "+905343133580"; // WhatsApp numarasını buraya ekleyin

            // WhatsApp arama URI'sini oluştur
            string whatsappAramaUri = "https://wa.me/" + telefonNumarasi;

            // Varsayılan web tarayıcısında WhatsApp arama URI'sini aç
            System.Diagnostics.Process.Start(whatsappAramaUri);
            btnWhatsappArama.Enabled = false;
        }

        private void btnSiradakiSoruJokeri_Click(object sender, EventArgs e)
        {

            GosterRastgeleSoru();
            btnSiradakiSoruJokeri.Enabled = false;
        }

        private void btnSureStop_Click(object sender, EventArgs e)
        {
            a = true;
            timer1.Stop();
            btnSureStop.Enabled=false;
        }
    }
}

    
    

