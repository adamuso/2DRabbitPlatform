using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform
{
    public class Poruszanie
    {
        // ---------------

        /*
         * Poruszanie, które jest tutaj przedstawione opiera się na przesuwaniu co pixel, z wektora prędkości oblicza ile pixeli powinna przesunąć się
         * postać. Ogólnie jest to zrobione gdyż kolizje są tutaj tzw. Pixel-Perfect, czy dokładnie sprawdzane po każdym pixelu. W przypadku stosowania
         * innych kolizji np. koła, prostokąty, zasada ruchu jest podobna. Albowiem:
         * - zapamiętuje się początkowe współrzedne X, Y
         * - przesuwa się je o podany wektor prędkości (Vx, Vy)
         * - sprawdza się kolizje, jeśli nastąpiła wracamy do początkowych pozycji
         * - jeśli kolizja nie nastąpiła to obliczone po dodaniu wektora prędkości stają się nowymi współrzędnymi
         */

        // -------------


        public override void Update(GameTime gt)
        {
            //base.Update(gt);

            /*if (IsAttached)
            {
                return;
            }*/

            if (gravityAffect)              // jeśli na obiekt oddziałuje grawitacja
                addGravityVelocity(gt);     // dodajemy grawitacje do Y
            bufferPosition();               // zapisujemy starą pozycje

            //slowdown(0.7f + (xFactor / 10f));

            moveHorizontal(gt);             // dlatego że to jest Pixel-Perfect przesuwamy osobno poziomo i pionowo
            onGround = moveVertical(gt);    // przesunięcie pionowe

            if (!this.Location.offset(0, this.CollisionArea.Height + 5).getTile().isAir)
                slowdown(0.7f + (xFactor / 10f));       // efekt tarcia i zatrzymywania się obiektu
            else
                slowdown(0.85f + 1 / mass + (xFactor / 20f));  // to samo tylko że na ziemi

            applyPosition();    // przypisujemy nową pozycje
        }

        public void moveHorizontal(GameTime gt)
        {
            int vel = (int)(Velocity.X * world.Game.getTimes(gt) * 10);     // uzyskaj ostateczną prędkość poprzez wyczulenie jej mnożąc przez 10 (doliczany czas tak naprawde nic nie zmienia)

            if (vel != 0)   // jeśli ma jakiś czas to wykonujemy ruch
            {
                int factor = vel / Math.Abs(vel);   // factor sluzy do oznaczenia ruchu w lewo i w prawo (lewo: -1f, prawo: 1f)
                vel = Math.Abs(vel);                // nastepnie usuwamy znak z prędkości aby mozna było jej użyć w normalnej pętli

                for (int i = 0; i < vel; i++)   // powtarzamy ilość ruchów jakie ma wykonać Entity
                {
                    move(0.1f * factor, 0);

                    if (world.CollisionManager.checkHorizontalMove(this, world.Level.Map) != 0)     // sprawdzenie kolizji
                    {
                        for (int s = 1; s <= 2; s++)
                        {
                            move(0, (int)(-s * world.Game.getTimes(gt)));

                            if (world.CollisionManager.checkHorizontalMove(this, world.Level.Map) == 0) // sprawdzenie kolizji
                            {
                                break;
                            }

                            if (s < 2)
                                continue;

                            resetYBuffer(); // ustawia Y na początkową wartość

                            move(-1 * factor, 0);
                            xvel = 0;
                            if (this is IEntityAI)
                                ((IEntityAI)this).AI.notify(AI.AICommunicationSymbols.AI_STOP, false);
                            goto escape;
                        }
                    }
                    else
                    {
                        for (int s = 1; s <= 3; s++)
                        {
                            move(0, 1);

                            if (world.CollisionManager.checkHorizontalMove(this, world.Level.Map) != 0)
                            {
                                move(0, -1);
                                break;
                            }

                            if (s == 3)
                                move(0, -s);
                        }
                    }

                }

            escape:
                position.Y = bufferedPosition.Y;
            }
        }

        public bool moveVertical(GameTime gt)
        {
            int vel = (int)(Velocity.Y * world.Game.getTimes(gt) * 10);

            if (vel != 0)
            {
                int factor = vel / Math.Abs(vel);
                vel = Math.Abs(vel);

                for (int i = 0; i < vel; i++)
                {
                    move(0, 0.1f * factor);
                    int height = world.CollisionManager.checkVerticalMove(this, world.Level.Map);

                    if (height != 0)
                    {
                        resetYBuffer();
                        resetYVelocity();

                        if (height > 0)
                        {
                            yvel += 1f;
                            move(0, 1);
                            jumpBoost = false;
                        }

                        if (height < 0)
                            return true;

                        break;
                    }
                }
            }

            return false;
        }
    }
}
