using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KMS_Sea_Wars
{
    public partial class Form1 : Form
    {
        public const int mapSize = 11;
        public int cellsSize = 25;
        public string alphabet = "АБВГДЕЖЗИК";

        public int[,] map = new int[mapSize, mapSize];
        public int[,] enemyMap = new int[mapSize, mapSize];
        public bool StartPlay = false;

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            this.Text = "Морской бой";
            StartPlay = false;
            Start();
        }

        public void Start()
        {
            CreateMap();
        }

        public void CreateMap()
        {
            for (int i = 0; i < mapSize; i++)
            {
                if (i == 0)
                {
                    for (int j = 1; j < mapSize; j++)
                    {
                        Label label = new Label();
                        label.Location = new Point(j * cellsSize, i * cellsSize);
                        label.Size = new Size(cellsSize, cellsSize);
                        label.Text = j.ToString();
                        this.Controls.Add(label);
                    }
                }
                else
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        if (j == 0)
                        {
                            Label label = new Label();
                            label.Location = new Point(j * cellsSize, i * cellsSize);
                            label.Size = new Size(cellsSize, cellsSize);
                            label.Text = alphabet[i - 1].ToString();
                            this.Controls.Add(label);
                        }
                        else
                        {
                            map[i, j] = 0;

                            Button button = new Button();
                            button.Location = new Point(j * cellsSize, i * cellsSize);
                            button.Size = new Size(cellsSize, cellsSize);
                            button.BackColor = Color.AliceBlue;
                            button.Click += new EventHandler(StartConfig);
                            this.Controls.Add(button);
                    }
                    }
                }
            }

            Button startButton = new Button();
            startButton.Text = "Начать игру";
            startButton.Location = new Point(cellsSize, mapSize * cellsSize + cellsSize);
            startButton.Click += new EventHandler(StartGame);
            this.Controls.Add(startButton);
        }

        public void StartGame(object sender, EventArgs e)
        {
            StartPlay = true;
            BotGeneration();
            this.Controls.Remove((Button)sender);
        }

        public void StartConfig(object sender, EventArgs e)
        {
            Button ship = sender as Button;

            if (!StartPlay)
            {
                if (map[ship.Location.Y / cellsSize, ship.Location.X / cellsSize] == 0)
                {
                    ship.BackColor = Color.BlueViolet;
                    map[ship.Location.Y / cellsSize, ship.Location.X / cellsSize] = 1;
                }
                else
                {
                    ship.BackColor = Color.AliceBlue;
                    map[ship.Location.Y / cellsSize, ship.Location.X / cellsSize] = 0;
                }
            }
        }

        public void PlayerShoot(object sender, EventArgs e)
        {
            Button shipshoot = sender as Button;
            Shoot(enemyMap, shipshoot);
        }

        public bool Shoot(int[,] map, Button ship)
        {
            bool hit = false;
            if (StartPlay)
            {
                if (map[ship.Location.Y / cellsSize, (ship.Location.X - 360) / cellsSize] != 0)
                {
                    hit = true;
                    ship.BackColor = Color.Coral;
                    ship.Text = "X";
                }
                else
                {
                    hit = false;
                    ship.BackColor = Color.MediumOrchid;

                }
            }
            return hit;
        }

        public void BotGeneration()
        {
            var rand = new Random();
            int x, y;
            int direction = 0;
            int countShips = 0;
            int shipLenght = 4;
            int counter = 0;

            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    enemyMap[i, j] = 0;
                }
            }

            while (countShips < 4)
            {
                x = rand.Next(1, 11);
                y = rand.Next(1, 11);

                int tempX = x;
                int tempY = y;

                direction = rand.Next(1, 5);

                bool setShip = true;

                //проверка
                for (int i = 0; i < 5; i++)
                {
                    if (tempX < 1 || tempY < 1 || tempX >= 11 || tempY >= 11)
                        {
                            setShip = false;
                            break;
                        }

                    if (enemyMap[tempY, tempX] == 1)
                    { 
                        setShip = false;
                        break;
                    }

                    if (tempY == 1 && tempX != 10)
                    {
                        if (
                             enemyMap[tempY, tempX + 1] == 1 ||
                             enemyMap[tempY + 1, tempX] == 1 ||
                             enemyMap[tempY + 1, tempX + 1] == 1 ||
                             enemyMap[tempY, tempX - 1] == 1 ||
                             enemyMap[tempY + 1, tempX - 1] == 1
                             )
                        {
                            setShip = false;
                            break;
                        }
                    }
                     
                    if (tempY == 10 && tempX != 10)
                    {
                        if (
                             enemyMap[tempY - 1, tempX] == 1 ||
                             enemyMap[tempY - 1, tempX - 1] == 1 ||
                             enemyMap[tempY - 1, tempX + 1] == 1 ||
                             enemyMap[tempY, tempX - 1] == 1 ||
                             enemyMap[tempY, tempX + 1] == 1
                             )
                        {
                            setShip = false;
                            break;
                        }
                    }

                    if (tempX == 1 && tempY != 10)
                    {
                        if (
                            enemyMap[tempY + 1, tempX] == 1 ||
                            enemyMap[tempY + 1, tempX + 1 ] == 1 ||
                            enemyMap[tempY, tempX + 1] == 1 ||
                            enemyMap[tempY - 1, tempX + 1] == 1 ||
                            enemyMap[tempY - 1, tempX] == 1
                           )
                        {
                            setShip = false;
                            break;
                        }
                    }

                    if (tempX == 10 && tempY != 10)
                    {
                        if (
                            enemyMap[tempY + 1, tempX] == 1 ||
                            enemyMap[tempY + 1, tempX - 1] == 1 ||
                            enemyMap[tempY, tempX - 1] == 1 ||
                            enemyMap[tempY - 1, tempX - 1] == 1 ||
                            enemyMap[tempY - 1, tempX] == 1
                           )
                        {
                            setShip = false;
                            break;
                        }
                    }

                    if (tempX > 1 && tempY > 1 && tempX < 10 && tempY < 10)
                    {
                        if (
                             enemyMap[tempY, tempX + 1] == 1 ||
                             enemyMap[tempY, tempX - 1] == 1 ||
                             enemyMap[tempY + 1, tempX] == 1 ||
                             enemyMap[tempY + 1, tempX + 1] == 1 ||
                             enemyMap[tempY + 1, tempX - 1] == 1 ||
                             enemyMap[tempY - 1, tempX] == 1 ||
                             enemyMap[tempY - 1, tempX + 1] == 1 ||
                             enemyMap[tempY - 1, tempX - 1] == 1
                             )
                        {
                            setShip = false;
                            break;
                        }
                    }

                    switch (direction)
                    {
                        case (1):
                            tempX += 1;
                            break;
                        case (2):
                            tempX -= 1;
                            break;
                        case (3):
                            tempY += 1;
                            break;
                        case (4):
                            tempY -= 1;
                            break;
                    }
               
                }


                //расстановка
                if (setShip)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        switch (direction)
                        {
                            case (1):
                                enemyMap[y, x] = 1;
                                x += 1;
                                break;
                            case (2):
                                enemyMap[y, x] = 1;
                                x -= 1;
                                break;
                            case (3):
                                enemyMap[y, x] = 1;
                                y += 1;
                                break;
                            case (4):
                                enemyMap[y, x] = 1;
                                y -= 1;
                                break;
                        }
                    }
                    countShips += 1;
                }
            }

                for (int i = 0; i < mapSize; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 1; j < mapSize; j++)
                        {
                            Label label = new Label();
                            label.Location = new Point(360 + j * cellsSize, i * cellsSize);
                            label.Size = new Size(cellsSize, cellsSize);
                            label.Text = j.ToString();
                            this.Controls.Add(label);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            if (j == 0)
                            {
                                Label label = new Label();
                                label.Location = new Point(360 + j * cellsSize, i * cellsSize);
                                label.Size = new Size(cellsSize, cellsSize);
                                label.Text = alphabet[i - 1].ToString();
                                this.Controls.Add(label);
                            }
                            else
                            {
                                Button button = new Button();
                                button.Location = new Point(360 + j * cellsSize, i * cellsSize);
                                button.Size = new Size(cellsSize, cellsSize);
                                button.Click += new EventHandler(PlayerShoot);
                                if (enemyMap[i, j] == 1)
                                {
                                    button.BackColor = Color.Red;
                                }
                                else
                                {
                                    button.BackColor = Color.AliceBlue;
                                }
                                this.Controls.Add(button);
                            }
                        }
                    }
                }
                
            

            }
        }
    }

