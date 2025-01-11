using UnityEngine;

public class GunSwitch : MonoBehaviour
{
    public GameObject assaultRifle; // Assault Rifle silahý
    public GameObject pistol; // Pistol silahý

    private GunController assaultRifleController;
    private GunControllerDeagle pistolController;

    void Start()
    {
        // Silahlarýn kontrol scriptlerini al
        assaultRifleController = assaultRifle.GetComponent<GunController>();
        pistolController = pistol.GetComponent<GunControllerDeagle>();

        // Baþlangýçta Assault Rifle aktif, Pistol pasif
        SelectAssaultRifle();
    }

    void Update()
    {
        // 1 tuþuna basýldýðýnda Assault Rifle'ye geçiþ yap
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAssaultRifle();
        }

        // 2 tuþuna basýldýðýnda Pistol'e geçiþ yap
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectPistol();
        }
    }

    void SelectAssaultRifle()
    {
        assaultRifle.SetActive(true);
        pistol.SetActive(false);

        // Assault Rifle'ýn mermi bilgisini güncelle
        assaultRifleController.UpdateAmmoUI();
    }

    void SelectPistol()
    {
        assaultRifle.SetActive(false);
        pistol.SetActive(true);

        // Pistol'ün mermi bilgisini güncelle
        pistolController.UpdateAmmoUI();
    }
}