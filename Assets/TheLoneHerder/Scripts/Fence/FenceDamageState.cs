using System.Collections;
using UnityEngine;
namespace TheLoneHerder
{
    public class FenceDamageState : FenceBaseState
    {
        FenceStateManager fence;
        int wolvesAttacking = 0;
        bool isHealthTicking = false;
        float tickSpeed = 0.2f;
        int savedHealth;

        public override void EnterState(FenceStateManager fenceRef)
        {
            fence = fenceRef;
            savedHealth = fence.Health;
        }

        public override void ExitState()
        {
            if (isHealthTicking)
            {
                fence.StopCoroutine(fence.damageHealth);
            }
        }

        public void IncrementWolvesAttacking(int fenceSide)
        {
            if (fenceSide == fence.side)
            {
                wolvesAttacking++;

                if (!isHealthTicking)
                {
                    fence.damageHealth = fence.StartCoroutine(DoDamage());
                }
            }
        }

        public void DecrementWolvesAttacking(int fenceSide)
        {
            if (fenceSide == fence.side)
            {
                wolvesAttacking--;
                if (wolvesAttacking <= 0)
                {
                    fence.StopCoroutine(fence.damageHealth);
                    isHealthTicking = false;
                    fence.SetDamagedHealth(savedHealth);
                }
            }
        }

        IEnumerator DoDamage()
        {
            isHealthTicking = true;
            if (wolvesAttacking > 0)
            {
                while (wolvesAttacking > 0 && savedHealth > 0)
                {
                    savedHealth--;
                    fence.SendUpdatedHealth(savedHealth);
                    fence.UpdateHealth(savedHealth);
                    yield return new WaitForSeconds(tickSpeed / wolvesAttacking);
                }
            }
            else
            {
                isHealthTicking = false;
            }
            if (savedHealth <= 0)
            {
                fence.DestroyFence();
            }
        }
    }
}