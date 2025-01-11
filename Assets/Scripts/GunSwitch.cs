using UnityEngine;

public class GunSwitch : MonoBehaviour
{
    public GameObject assaultRifle; // Assault Rifle silah�
    public GameObject pistol; // Pistol silah�

    private GunController assaultRifleController;
    private GunControllerDeagle pistolController;

    void Start()
    {
        // Silahlar�n kontrol scriptlerini al
        assaultRifleController = assaultRifle.GetComponent<GunController>();
        pistolController = pistol.GetComponent<GunControllerDeagle>();

        // Ba�lang��ta Assault Rifle aktif, Pistol pasif
        SelectAssaultRifle();
    }

    void Update()
    {
        // 1 tu�una bas�ld���nda Assault Rifle'ye ge�i� yap
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAssaultRifle();
        }

        // 2 tu�una bas�ld���nda Pistol'e ge�i� yap
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectPistol();
        }
    }

    void SelectAssaultRifle()
    {
        assaultRifle.SetActive(true);
        pistol.SetActive(false);

        // Assault Rifle'�n mermi bilgisini g�ncelle
        assaultRifleController.UpdateAmmoUI();
    }

    void SelectPistol()
    {
        assaultRifle.SetActive(false);
        pistol.SetActive(true);

        // Pistol'�n mermi bilgisini g�ncelle
        pistolController.UpdateAmmoUI();
    }
}