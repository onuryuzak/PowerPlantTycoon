using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoneyChangeEvent", menuName = "Artera/Event/MoneyChangeEvent")]
public class MoneyChangeEventSO : ScriptableObject
{
    public UnityEvent<int> onMoneyChangeListener;
}