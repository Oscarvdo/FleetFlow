using FleetFlow.Application.Abstractions.Security;
using FleetFlow.Application.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace FleetFlow.Dispatch.WinForms.Forms.Authentication;

public partial class LoginForm : Form
{
    private readonly IAuthenticationService? _authenticationService;

    public LoginForm()
    {
        InitializeComponent();
    }

    public LoginForm(
        IAuthenticationService authenticationService)
        : this()
    {
        _authenticationService = authenticationService;
    }

    public UserSession? AuthenticatedSession { get; private set; }

    private void chkShowPassword_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        txtPassword.UseSystemPasswordChar =
            !chkShowPassword.Checked;
    }

    private async void btnLogin_Click(
        object? sender,
        EventArgs e)
    {
        lblError.Visible = false;

        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            ShowError("Enter your username and password.");
            return;
        }

        if (_authenticationService is null)
        {
            ShowError("Authentication service is unavailable.");
            return;
        }

        SetBusyState(true);

        try
        {
            LoginResult result =
                await _authenticationService.AuthenticateAsync(
                    new LoginRequest(username, password));

            if (!result.Succeeded ||
                result.Session is null)
            {
                ShowError(
                    result.ErrorMessage ??
                    "Unable to sign in.");

                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            AuthenticatedSession = result.Session;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception)
        {
            ShowError(
                "Unable to connect to FleetFlow. Try again.");
        }
        finally
        {
            SetBusyState(false);
        }
    }

    private void SetBusyState(bool isBusy)
    {
        txtUsername.Enabled = !isBusy;
        txtPassword.Enabled = !isBusy;
        chkShowPassword.Enabled = !isBusy;
        btnLogin.Enabled = !isBusy;
        btnLogin.Text = isBusy
            ? "SIGNING IN..."
            : "SIGN IN";

        UseWaitCursor = isBusy;
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }
}