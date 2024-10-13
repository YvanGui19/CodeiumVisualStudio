﻿using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CodeiumVS;

[ComVisible(true)]
public class SettingsPage : DialogPage
{
    public const string CategoryName = "Codeium";

    private EnterpriseModeEnum enterpriseMode;
    private string portalUrl = "";
    private string apiUrl = "";
    private string extensionBaseUrl = "https://github.com/Exafunction/codeium/releases/download";
    private bool enableCommentCompletion = true;
    private bool enableLanguageServerProxy = false;
    private bool enableIndexing = true;
    private bool enableCodeLens = true;
    private int indexingMaxFileCount = 5000;

    public enum EnterpriseModeEnum
    {
        [Description("False if using Visual Studio with Codeium Individual. Requires restart.")]
        False,
        [Description("True if using Visual Studio with Codeium Enterprise. Requires restart.")]
        True,
        [Description("Open if using Visual Studio with Open Source LLM. Requires restart.")]
        Open
    }

    [Category(CategoryName)]
    [DisplayName("Enterprise Mode")]
    [Description(
        "Set this to True if using Visual Studio with Codeium Enterprise. Requires restart.")]
    public EnterpriseModeEnum EnterpriseMode
    {
        get {
            return enterpriseMode;
        }
        set {
            enterpriseMode = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Portal Url")]
    [Description("URL of the Codeium Enterprise Portal. Requires restart.")]
    public string PortalUrl
    {
        get {
            return portalUrl;
        }
        set {
            portalUrl = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Language Server Download URL")]
    [Description(
        "If you're experiencing network issues with GitHub and can't download the language server, please change this to a GitHub Mirror URL instead. For example: https://gh.api.99988866.xyz/https://github.com/Exafunction/codeium/releases/download")]
    public string ExtensionBaseUrl
    {
        get {
            return extensionBaseUrl;
        }
        set {
            extensionBaseUrl = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("API Url")]
    [Description("API Url for Codeium Enterprise. Requires restart.")]
    public string ApiUrl
    {
        get {
            return apiUrl;
        }
        set {
            apiUrl = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Enable comment completion")]
    [Description("Whether or not Codeium will provide completions for comments.")]
    public bool EnableCommentCompletion
    {
        get {
            return enableCommentCompletion;
        }
        set {
            enableCommentCompletion = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Enable Code Lens")]
    [Description("AI-powered inline action buttons in your editor. (Reload Required)")]
    public bool EnableCodeLens
    {
        get
        {
            return enableCodeLens;
        }
        set
        {
            enableCodeLens = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Enable language server proxy")]
    [Description(
        "If you're experiencing network issues with the language server, we recommend enabling this option and using a VPN to resolve the issue. Requires restart.")]
    public bool EnableLanguageServerProxy
    {
        get {
            return enableLanguageServerProxy;
        }
        set {
            enableLanguageServerProxy = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Enable Codeium Indexing")]
    [Description(
        "Allows Codeium to index your current repository and provide better chat and autocomplete responses based on relevant parts of your codebase. Requires restart.")]
    public bool EnableIndexing
    {
        get {
            return enableIndexing;
        }
        set {
            enableIndexing = value;
        }
    }

    [Category(CategoryName)]
    [DisplayName("Indexing Max Workspace Size (File Count)")]
    [Description(
        "If indexing is enabled, we will only attempt to index workspaces that have up to this many files. This file count ignores .gitignore and binary files.")]
    public int IndexingMaxWorkspaceSize
    {
        get {
            return indexingMaxFileCount;
        }
        set {
            indexingMaxFileCount = value;
        }
    }
}
