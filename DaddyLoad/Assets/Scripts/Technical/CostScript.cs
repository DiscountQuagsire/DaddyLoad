using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostScript
{
    
    public static Inventory getCost(string upgrade, int level)
    {
        Inventory output = new Inventory();

        switch (upgrade)
        {
            case "cockpit":
                switch (level)
                {
                    case 1:
                        output.addMaterial("copper", 1); break;
                    case 2:
                        output.addMaterial("copper", 2); break;
                }
                break;
            

            case "reactor":
                switch (level)
                {
                    case 1:
                        output.addMaterial("gold", 1); break;
                    case 2:
                        output.addMaterial("gold", 2); break;
                    case 3:
                        output.addMaterial("gold", 3); break;
                    case 4:
                        output.addMaterial("gold", 4); break;

                }
                break;


            case "room1":
                switch (level)
                {
                    case 1:
                            output.addMaterial("silver", 1); break;
                    case 2:
                            output.addMaterial("silver", 2); break;
                    case 3:
                            output.addMaterial("silver", 3); break;
                    case 4:
                            output.addMaterial("silver", 4); break;
                }
                break;


            case "room2":
                switch (level)
                {
                    case 1:
                        output.addMaterial("plastic", 1); break;
                    case 2:
                        output.addMaterial("plastic", 2); break;
                    case 3:
                        output.addMaterial("plastic", 3); break;
                    case 4:
                        output.addMaterial("plastic", 4); break;
                }
                break;


            case "room3":
                switch (level)
                {
                    case 1:
                        output.addMaterial("aluminum", 1); break;
                    case 2:
                        output.addMaterial("aluminum", 2); break;
                    case 3:
                        output.addMaterial("aluminum", 3); break;
                    case 4:
                        output.addMaterial("aluminum", 4); break;
                }
                break;


            case "room4":
                switch (level)
                {
                    case 1:
                        output.addMaterial("emerald", 1); break;
                    case 2:
                        output.addMaterial("emerald", 2); break;
                    case 3:
                        output.addMaterial("emerald", 3); break;
                    case 4:
                        output.addMaterial("emerald", 4); break;
                }
                break;


            case "room5":
                switch (level)
                {
                    case 1:
                        output.addMaterial("ruby", 1); break;
                    case 2:
                        output.addMaterial("ruby", 2); break;
                    case 3:
                        output.addMaterial("ruby", 3); break;
                    case 4:
                        output.addMaterial("ruby", 4); break;
                }
                break;


            case "room6":
                switch (level)
                {
                    case 1:
                        output.addMaterial("pinkore", 1); break;
                    case 2:
                        output.addMaterial("pinkore", 2); break;
                    case 3:
                        output.addMaterial("pinkore", 3); break;
                    case 4:
                        output.addMaterial("pinkore", 4); break;
                }
                break;

            default:
                output.addMaterial("copper", 1);
                break;

        }

       
        return output;
    }

}
