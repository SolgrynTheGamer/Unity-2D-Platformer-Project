using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // ȯ�漳�� Panel GameObject�� ������ ����

    // --- ���� ���� ���� ���� ---
    public AudioMixer gameMixer; // Project �信�� ������ Audio Mixer�� ����
    public Slider masterVolumeSlider; // ������ ���� �����̴� UI�� ����
    private const string MASTER_VOLUME_PARAM = "MasterVolume"; // Audio Mixer�� Exposed Parameter �̸��� �����ϰ�

    // --- �ػ� ���� ���� ���� ---
    public Dropdown resolutionDropdown; // �ػ� ��Ӵٿ� UI�� ����
    private List<Resolution> resolutions; // ���� �ý��ۿ��� �����ϴ� �ػ� ���

    void Start()
    {
        // ȯ�漳�� Panel�� ��Ȱ��ȭ�Ǿ� �ִ��� Ȯ�� (�����Ϳ��� �Ǽ��� ���� �������)
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // --- ���� ���� �ʱ�ȭ �� �ε� ---
        InitializeVolumeSettings();

        // --- �ػ� ���� �ʱ�ȭ �� �ε� ---
        InitializeResolutionSettings();
    }

    // --- ȯ�漳�� â ����/�ݱ� �Լ� ---
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Panel Ȱ��ȭ
        }
    }

    public void CloseSettings()
    {
        // ���� �ݱ� ���� ������� ����
        PlayerPrefs.Save();
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Panel ��Ȱ��ȭ
        }
    }

    // --- ���� ���� �Լ� ---
    private void InitializeVolumeSettings()
    {
        if (masterVolumeSlider == null || gameMixer == null) return;

        if (PlayerPrefs.HasKey(MASTER_VOLUME_PARAM))
        {
            float savedVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM);
            SetMasterVolume(savedVolume); // Audio Mixer�� ����
            masterVolumeSlider.value = savedVolume; // �����̴� UI�� ����
        }
        else
        {
            // ����� ���� ������ �⺻�� ���� (��: 1.0f, �ִ�)
            SetMasterVolume(1.0f);
            masterVolumeSlider.value = 1.0f;
        }
    }

    public void SetMasterVolume(float value)
    {
        // �����̴��� ���� ��(0~1)�� Audio Mixer�� �α� ������(-80dB~0dB)�� ��ȯ
        // value�� 0�� �� Log10(0)�� ���Ѵ밡 �ǹǷ�, ���� ��(0.0001)���� ����
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        gameMixer.SetFloat(MASTER_VOLUME_PARAM, volume);

        // ���� ���� ���� PlayerPrefs�� ����
        PlayerPrefs.SetFloat(MASTER_VOLUME_PARAM, value);
    }

    // --- �ػ� ���� �Լ� ---
    private void InitializeResolutionSettings()
    {
        if (resolutionDropdown == null) return;

        resolutions = new List<Resolution>();
        // ���� ����Ͱ� �����ϴ� ��� �ػ� ��������
        foreach (Resolution res in Screen.resolutions)
        {
            // �ߺ� �ػ� ���� (���� �ػ󵵿� �ٸ� �ֻ����� ���� �� �����Ƿ�)
            bool found = false;
            foreach (Resolution existingRes in resolutions)
            {
                if (existingRes.width == res.width && existingRes.height == res.height)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                resolutions.Add(res);
            }
        }

        // ���� �ػ󵵺��� ���̵��� ���� ���� (���� ����)
        resolutions.Reverse();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0; // ��Ӵٿ ���� ����� �ػ󵵸� ǥ���ϱ� ���� �ε���

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // ���� �ý��� �ػ󵵿� ��ġ�ϴ� �ػ󵵸� ã�Ƽ� �⺻ ���ð����� ����
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions(); // ���� �ɼ� �ʱ�ȭ
        resolutionDropdown.AddOptions(options); // �� �ɼ� �߰�
        resolutionDropdown.value = currentResolutionIndex; // ���� �ػ󵵷� �ʱ� ����
        resolutionDropdown.RefreshShownValue(); // ��Ӵٿ� UI ������Ʈ

        // ����� �ػ� ���� �ε�
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            int savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
            if (savedResolutionIndex >= 0 && savedResolutionIndex < resolutions.Count)
            {
                SetResolution(savedResolutionIndex); // ����� �ػ󵵷� ����
                resolutionDropdown.value = savedResolutionIndex; // ��Ӵٿ� UI���� ����
                resolutionDropdown.RefreshShownValue();
            }
        }
        else
        {
            // ����� ���� ������ ���� �ػ󵵷� ����
            SetResolution(currentResolutionIndex);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Count) return;

        Resolution resolution = resolutions[resolutionIndex];
        // Screen.fullScreen�� ���� Ǯ��ũ�� ��� ���θ� �����մϴ�.
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // �ػ� �ε����� PlayerPrefs�� ����
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }
}