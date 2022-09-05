using System.Media;

namespace Lock_Picking
{
    public class Game
    {
        //font
        private static readonly Font font = new(FontFamily.GenericMonospace, 25, FontStyle.Bold);
        private static readonly StringFormat Format = new();
        //sounds
        private readonly SoundPlayer lockPickingEnter = new(Resource.lockpicking_enter);
        private readonly SoundPlayer lockPickingFail = new(Resource.lockpicking_fail);
        private readonly SoundPlayer lockPickingSuccess = new(Resource.lockpicking_unlock);
        private readonly SoundPlayer lockPickingMove = new(Resource.lockpicking_pickmovement);
        //images
        private readonly Bitmap masterKey = Resource.Lockpick;
        private readonly Bitmap lockpick = Resource.Screwdriver;
        private readonly Bitmap keyHole = Resource.Keyhole;

        public static Random Random = new();
        private readonly int width;
        private readonly int height;

        private int strength;
        private int brakingMasterKey;
        private int breakingLock;

        private double lockpickAngle;
        private int screwdriverAngle;
        private int screwdriverAngleLimit;

        private int targetAngle;
        private int targetZone = 10;
        private int targetSecondZone = 25;

        public bool isKeyPressed;

        public Game(int gameFiledWidth, int gameFieldHeight)
        {
            lockPickingEnter.Play();

            width = gameFiledWidth;
            height = gameFieldHeight;
            Format.Alignment = StringAlignment.Center;
            strength = 10;

            Restart();
        }

        public void Update(int mouseX)
        {
            //todo
            lockpickAngle = 180 * mouseX / width - 180;

            //TODO
            if (lockpickAngle < targetAngle + targetZone &&
                lockpickAngle > targetAngle - targetZone)
                screwdriverAngleLimit = 90;
            else if (lockpickAngle < targetAngle + targetSecondZone &&
                     lockpickAngle > targetAngle - targetSecondZone)
                screwdriverAngleLimit = 30;
            else
                screwdriverAngleLimit = 0;

            if (isKeyPressed)
            {
                if (screwdriverAngle <= screwdriverAngleLimit)
                {
                    screwdriverAngle += 4;
                    if (screwdriverAngle >= 90)
                    {
                        lockPickingSuccess.PlaySync();
                        breakingLock++;
                        Restart();
                        targetAngle = GetRandomTargetAngle();
                    }
                }
                else
                {
                    screwdriverAngle -= 4;
                    lockPickingMove.Play();
                    if (strength > 1)
                        strength--;
                    else
                    {
                        lockPickingFail.PlaySync();
                        brakingMasterKey++;
                        Restart();
                        strength = 10;
                    }
                }
            }
            else
            if (screwdriverAngle >= 0)
                screwdriverAngle -= 4;

        }
        //TODO
        public void Draw(Graphics graphics)
        {
            string text1 = $"Взломанные замки:{breakingLock}" +
                           $"\nСломанные отмычки:{brakingMasterKey}" +
                           $"\nПрочность отмычки:{strength}";

            graphics.DrawString(text1, font, Brushes.Black, width / 2, height / 12, Format);


            int X = width / 2;
            int Y = height / 2;

            double temp = Math.Sqrt(keyHole.Width * keyHole.Width + keyHole.Height * keyHole.Height) / 2;

            Point[] destinationPointsKeyHole =
            {
                new((int)(X + temp * Math.Cos((screwdriverAngle - 135) * Math.PI / 180)),
                    (int)(Y + 10 + temp * Math.Sin((screwdriverAngle - 135) * Math.PI / 180))),
                new((int)(X + temp * Math.Cos((screwdriverAngle - 45) * Math.PI / 180)),
                    (int)(Y + 10 + temp * Math.Sin((screwdriverAngle - 45) * Math.PI / 180))),
                new((int)(X + temp * Math.Cos((screwdriverAngle + 135) * Math.PI / 180)),
                    (int)(Y + 10 + temp * Math.Sin((screwdriverAngle + 135) * Math.PI / 180)))
            };

            graphics.DrawImage(keyHole, destinationPointsKeyHole);
            Point[] destinationPoints =
            {
                new((int)(X - lockpick.Width / 2 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180)),
                    (int)(Y - lockpick.Width / 2 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180))),
                new((int)(X + lockpick.Width / 2 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180)),
                    (int)(Y + lockpick.Width / 2 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180))),
                new((int)(X + lockpick.Height * Math.Cos(screwdriverAngle * Math.PI / 180)),
                    (int)(Y + lockpick.Height * Math.Sin(screwdriverAngle * Math.PI / 180)))
            };
            graphics.DrawImage(lockpick, destinationPoints);

            double masterKeyX = X + 20 * Math.Cos((screwdriverAngle - 90) * Math.PI / 180);
            double masterKeyY = Y + 20 * Math.Sin((screwdriverAngle - 90) * Math.PI / 180);

            Point[] destinationPointsKey =
            {
                new((int)(masterKeyX - masterKey.Width / 2 * Math.Cos((lockpickAngle - 90) * Math.PI / 180)),
                    (int)(masterKeyY - masterKey.Width / 2 * Math.Sin((lockpickAngle - 90) * Math.PI / 180))),
                new((int)(masterKeyX + masterKey.Width / 2 * Math.Cos((lockpickAngle - 90) * Math.PI / 180)),
                    (int)(masterKeyY + masterKey.Width / 2 * Math.Sin((lockpickAngle - 90) * Math.PI / 180))),
                new((int)(masterKeyX + masterKey.Height * Math.Cos(lockpickAngle * Math.PI / 180)),
                    (int)(masterKeyY + masterKey.Height * Math.Sin(lockpickAngle * Math.PI / 180)))
            };

            graphics.DrawImage(masterKey, destinationPointsKey);

            graphics.DrawImage(lockpick, destinationPoints);
        }

        private void Restart()
        {
            isKeyPressed = false;
            screwdriverAngle = 0;
            lockpickAngle = -90;
        }

        private int GetRandomTargetAngle() => Random.Next(-180 + targetSecondZone, -targetSecondZone);
    }
}
