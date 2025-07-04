#!/usr/bin/env python3
import os
import re

def fix_taskstatus_in_file(filepath):
    """Fix TaskStatus references in a single file."""
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    original_content = content
    
    # Pattern to match TaskStatus.XXX where XXX is Todo, InProgress, Completed, or Cancelled
    # But not if it's already prefixed with the full namespace
    pattern = r'(?<!TaskManagementSystem\.Web\.Models\.)TaskStatus\.(Todo|InProgress|Completed|Cancelled)'
    replacement = r'TaskManagementSystem.Web.Models.TaskStatus.\1'
    
    content = re.sub(pattern, replacement, content)
    
    if content != original_content:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Fixed: {os.path.basename(filepath)}")
        return True
    return False

def process_directory(directory):
    """Process all .cshtml files in directory and subdirectories."""
    fixed_count = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cshtml'):
                filepath = os.path.join(root, file)
                if fix_taskstatus_in_file(filepath):
                    fixed_count += 1
    return fixed_count

# Main execution
web_views_path = "/Users/bilawalrizky/Documents/2025 - Project/SAT/TaskManagementSystem/TaskManagementSystem.Web/Views"
print(f"Fixing TaskStatus references in {web_views_path}...")
fixed_files = process_directory(web_views_path)
print(f"\nFixed {fixed_files} files!")

# Also update the Web.csproj file to update the vulnerable Refit package
csproj_path = "/Users/bilawalrizky/Documents/2025 - Project/SAT/TaskManagementSystem/TaskManagementSystem.Web/TaskManagementSystem.Web.csproj"
if os.path.exists(csproj_path):
    with open(csproj_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Update Refit package version
    content = re.sub(r'<PackageReference Include="Refit" Version="7\.2\.0"', 
                     '<PackageReference Include="Refit" Version="7.2.22"', content)
    
    with open(csproj_path, 'w', encoding='utf-8') as f:
        f.write(content)
    print("\nUpdated Refit package to version 7.2.22 to fix security vulnerability")
