using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private GameObject[] weapons;
    [SerializeField] private float switchDelay = 1f;

    private int currentIndex;
    private bool isSwitching;

	// Use this for initialization
	void Start ()
    {
        InitWeapons();
	}

    private void InitWeapons()
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            currentIndex++;

            if(currentIndex >= weapons.Length)
            {
                currentIndex = 0;
            }
            StartCoroutine(SwitchAfterDelay(currentIndex));
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = weapons.Length - 1;
            }
            StartCoroutine(SwitchAfterDelay(currentIndex));
        }
    }

    private void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[newIndex].SetActive(true);
    }

    private IEnumerator SwitchAfterDelay(int newIndex)
    {
        isSwitching = true;
        yield return new WaitForSeconds(switchDelay);
        SwitchWeapons(newIndex);
        isSwitching = false;
    }

}
