using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;

class Program
{
    static System.Windows.Forms.Timer creationTimer;
    static Random random = new Random();

    // 預設的 URL 圖片列表
    static List<string> imageUrls = new List<string>
    {
        "https://assets.coingecko.com/coins/images/39471/large/Skibidi_Toilet.png?1722401492",
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTZm2AAUgQqzMM7dbx4l9jUW-acl4cDQ9xKTw&s",
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhDFi23iJ1E-HQa18_gT6aFzAwER_Qa2wx0w&s",
        "https://i.ytimg.com/vi/joBk57gLyGk/hqdefault.jpg",
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJWGFGgRF6gYgPN2Ynd3raoAtkx3BwcG0GAg&s",
        "https://i.ytimg.com/vi/cp2Cman3IK8/hq720.jpg",
        "https://img.freepik.com/premium-photo/anime-sigma-boy-with-green-eyes_991087-1049.jpg",
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT19jzagsnYAJKdeYSnXhnefvTalN06AQnFNg&s",
        "https://imgcdn.stablediffusionweb.com/2024/3/17/3dc94a28-83bd-4f7c-b33e-71652870473a.jpg",
        "https://i.ytimg.com/vi/PPgqEcV5LIA/sddefault.jpg",
        "https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/2920270/capsule_616x353.jpg?t=1725626076",
        "https://s3.amazonaws.com/pix.iemoji.com/images/emoji/apple/ios-12/256/skull.png"
    };

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();

        // 設置每秒創建一個新視窗的計時器
        creationTimer = new System.Windows.Forms.Timer
        {
            Interval = 1000 // 每秒顯示一個新視窗
        };
        creationTimer.Tick += (sender, e) => CreateRandomImageUrlForm();
        creationTimer.Start();

        Application.Run();
    }

    // 創建隨機 URL 圖片視窗
    private static void CreateRandomImageUrlForm()
    {
        string randomImageUrl = imageUrls[random.Next(imageUrls.Count)];

        Form imageForm = new Form
        {
            Text = "Random SUS",
            Size = new Size(400, 300),
            StartPosition = FormStartPosition.Manual,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            TopMost = true,
            Location = GetRandomLocation()
        };

        PictureBox pictureBox = new PictureBox
        {
            SizeMode = PictureBoxSizeMode.StretchImage,
            Dock = DockStyle.Fill
        };

        // 嘗試載入圖片
        try
        {
            using (WebClient client = new WebClient())
            {
                var stream = client.OpenRead(randomImageUrl);
                pictureBox.Image = Image.FromStream(stream);
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Failed to load image from URL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        imageForm.Controls.Add(pictureBox);

        // 設置視窗自動移動
        int dx = random.Next(3, 7) * (random.Next(2) == 0 ? 1 : -1); // 隨機水平移動速度
        int dy = random.Next(3, 7) * (random.Next(2) == 0 ? 1 : -1); // 隨機垂直移動速度

        Timer moveTimer = new Timer { Interval = 30 }; // 設置移動間隔
        moveTimer.Tick += (s, e) =>
        {
            var screen = Screen.PrimaryScreen.WorkingArea;

            // 計算新位置
            var newX = imageForm.Left + dx;
            var newY = imageForm.Top + dy;

            // 檢查邊緣反彈
            if (newX < 0 || newX + imageForm.Width > screen.Width)
                dx = -dx; // 反轉水平移動方向
            if (newY < 0 || newY + imageForm.Height > screen.Height)
                dy = -dy; // 反轉垂直移動方向

            // 更新視窗位置
            imageForm.Left += dx;
            imageForm.Top += dy;
        };

        moveTimer.Start();

        // 當視窗關閉時，創建兩個新的隨機 URL 圖片視窗
        imageForm.FormClosed += (s, e) =>
        {
            moveTimer.Stop();
            CreateRandomImageUrlForm();
            CreateRandomImageUrlForm();
        };

        imageForm.Show();
    }

    private static Point GetRandomLocation()
    {
        var screen = Screen.PrimaryScreen.WorkingArea;
        return new Point(random.Next(screen.Width - 400), random.Next(screen.Height - 300));
    }
}
