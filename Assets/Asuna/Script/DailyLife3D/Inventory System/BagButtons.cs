using UnityEngine;
using UnityEngine.Serialization;
public class BagButtons : MonoBehaviour
{
    [FormerlySerializedAs("Apanel")] [SerializeField] private GameObject aPanel;
    [FormerlySerializedAs("Bpanel")] [SerializeField] private GameObject bPanel;
    [FormerlySerializedAs("Cpanel")] [SerializeField] private GameObject cPanel;
    [FormerlySerializedAs("Dpanel")] [SerializeField] private GameObject dPanel;
    [FormerlySerializedAs("Epanel")] [SerializeField] private GameObject ePanel;
    [FormerlySerializedAs("Fpanel")] [SerializeField] private GameObject fPanel;
    [FormerlySerializedAs("Gpanel")] [SerializeField] private GameObject gPanel;
    [FormerlySerializedAs("Hpanel")] [SerializeField] private GameObject hPanel;
    [FormerlySerializedAs("Ipanel")] [SerializeField] private GameObject iPanel;
    [FormerlySerializedAs("jpanel")] [SerializeField] private GameObject jPanel;
    private GameObject _currentPanel;

    private void Awake()
    {
        _currentPanel = aPanel;
        transform.parent.gameObject.SetActive(false);
    }
    private void SwitchPanel(GameObject panel)
    {
        if (panel == null || panel == _currentPanel)
            return;

        if (_currentPanel != null)
            _currentPanel.SetActive(false);

        panel.SetActive(true);
        _currentPanel = panel;
    }
    public void A() => SwitchPanel(aPanel);
    public void B() => SwitchPanel(bPanel);
    public void C() => SwitchPanel(cPanel);
    public void D() => SwitchPanel(dPanel);
    public void E() => SwitchPanel(ePanel);
    public void F() => SwitchPanel(fPanel);
    public void G() => SwitchPanel(gPanel);
    public void H() => SwitchPanel(hPanel);
    public void I() => SwitchPanel(iPanel);
    public void J() => SwitchPanel(jPanel);
}
