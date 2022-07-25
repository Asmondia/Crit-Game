using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Element { Normal, Fire, Water, Grass, Electric, Ice, Fighting, Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy, None};
public static class Type
{
    

    public static float Weakness(Element attackingType, Element defendingType)
    {
        Element[] weakness;
        Element[] resistance;
        Element[] immunity;
        switch (defendingType)
        {

            case (Element.Normal):
                weakness = new Element[] { Element.Fighting };
                resistance = new Element[] { };
                immunity = new Element[] { Element.Ghost};
                break;
            case (Element.Fire):
                weakness = new Element[] { Element.Water, Element.Ground, Element.Rock};
                resistance = new Element[] { Element.Fire, Element.Grass,Element.Ice, Element.Bug, Element.Steel, Element.Fairy };
                immunity = new Element[] { };
                break;
            case (Element.Water):
                weakness = new Element[] { Element.Electric, Element.Grass };
                resistance = new Element[] { Element.Fire, Element.Water, Element.Ice, Element.Steel };
                immunity = new Element[] { };
                break;
            case (Element.Grass):
                weakness = new Element[] { Element.Fire, Element.Ice, Element.Poison, Element.Flying, Element.Bug };
                resistance = new Element[] { Element.Grass };
                immunity = new Element[] { };
                break;
            case (Element.Electric):
                weakness = new Element[] { Element.Ground };
                resistance = new Element[] { Element.Electric, Element.Flying, Element.Steel };
                immunity = new Element[] { };
                break;
            case (Element.Ice):
                weakness = new Element[] { Element.Fire, Element.Fighting, Element.Rock, Element.Steel };
                resistance = new Element[] { Element.Ice };
                immunity = new Element[] { };
                break;
            case (Element.Fighting):
                weakness = new Element[] { Element.Flying, Element.Psychic, Element.Fairy };
                resistance = new Element[] {Element.Bug, Element.Rock, Element.Dark };
                immunity = new Element[] { };
                break;
            case (Element.Poison):
                weakness = new Element[] { Element.Ground, Element.Psychic };
                resistance = new Element[] { Element.Grass, Element.Fighting, Element.Poison, Element.Bug, Element.Psychic};
                immunity = new Element[] { };
                break;
            case (Element.Ground):
                weakness = new Element[] { Element.Water, Element.Grass, Element.Ice };
                resistance = new Element[] { Element.Poison, Element.Rock};
                immunity = new Element[] { Element.Electric};
                break;
            case (Element.Flying):
                weakness = new Element[] { Element.Ice, Element.Electric, Element.Rock};
                resistance = new Element[] { Element.Grass, Element.Fighting, Element.Bug };
                immunity = new Element[] { Element.Ground };
                break;
            case (Element.Psychic):
                weakness = new Element[] { Element.Bug, Element.Ghost, Element.Dark};
                resistance = new Element[] { Element.Fighting, Element.Psychic };
                immunity = new Element[] { };
                break;
            case (Element.Bug):
                weakness = new Element[] { Element.Fire, Element.Flying, Element.Rock };
                resistance = new Element[] { Element.Grass, Element.Fighting, Element.Ground };
                immunity = new Element[] { };
                break;
            case (Element.Rock):
                weakness = new Element[] { Element.Water, Element.Grass, Element.Fighting, Element.Ground, Element.Steel };
                resistance = new Element[] { Element.Normal, Element.Fire, Element.Poison, Element.Flying };
                immunity = new Element[] { };
                break;
            case (Element.Ghost):
                weakness = new Element[] { Element.Ghost, Element.Dark };
                resistance = new Element[] { Element.Poison, Element.Bug  };
                immunity = new Element[] { Element.Normal, Element.Fighting};
                break;
            case (Element.Dragon):
                weakness = new Element[] { Element.Ice, Element.Dragon, Element.Fairy };
                resistance = new Element[] { Element.Fire, Element.Water, Element.Electric, Element.Grass };
                immunity = new Element[] { };
                break;
            case (Element.Dark):
                weakness = new Element[] { Element.Fighting, Element.Bug, Element.Fairy };
                resistance = new Element[] { Element.Ghost, Element.Dark};
                immunity = new Element[] { Element.Psychic };
                break;
            case (Element.Steel):
                weakness = new Element[] { Element.Fire, Element.Fighting, Element.Ground };
                resistance = new Element[] { Element.Normal, Element.Grass, Element.Ice, Element.Flying, Element.Psychic, Element.Bug, Element.Rock, Element.Dragon, Element.Steel, Element.Fairy };
                immunity = new Element[] { Element.Poison};
                break;
            case (Element.Fairy):
                weakness = new Element[] { Element.Poison, Element.Steel };
                resistance = new Element[] { Element.Fighting, Element.Bug, Element.Dark};
                immunity = new Element[] { Element.Dragon};
                break;

            default:
                return 1f;

        }
        return weaknessCalc(attackingType, defendingType, weakness, resistance, immunity);
    }

    private static float weaknessCalc(Element attackingType, Element defendingType, Element[] weaknessList, Element[] resistanceList , Element[] immunityList)
    {
        if (Array.Exists<Element>(weaknessList, element => element.Equals(attackingType)))
        {
            return 2f;
        }
        else if (Array.Exists(resistanceList, element => element.Equals(attackingType)))
        {
            return 0.5f;
        }
        else if (Array.Exists(immunityList, element => element.Equals(attackingType)))
        {
            return 0f;
        }
        {
            return 1f;
        }
    }
}
