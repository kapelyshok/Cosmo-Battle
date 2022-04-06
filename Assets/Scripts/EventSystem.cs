using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystem : MonoBehaviour
{
    static public UnityEvent<int> playerTookDamage= new UnityEvent<int>();

    static public UnityEvent enoughGoldForShield = new UnityEvent();

    static public UnityEvent shieldWasUpgraded = new UnityEvent();

    static public UnityEvent gameIsOver = new UnityEvent();
    static public void sendEventPlayerTookDamage(int damage)
    {
        playerTookDamage.Invoke(damage);
    }
    static public void sendEnoughGoldForShield()
    {
        enoughGoldForShield.Invoke();
    }
    static public void sendShieldWasUpgraded()
    {
        shieldWasUpgraded.Invoke();
    }
    static public void sendGameIsOver()
    {
        gameIsOver.Invoke();
    }
}
