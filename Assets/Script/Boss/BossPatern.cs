using System.Collections;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour
{
    protected BossController boss;

    public virtual void Initialize(BossController bossController)
    {
        boss = bossController;
    }

    public abstract IEnumerator ExecutePattern(int currentHP, int maxHP);
}
