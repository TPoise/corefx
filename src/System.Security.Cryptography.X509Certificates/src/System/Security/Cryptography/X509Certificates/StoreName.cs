// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace System.Security.Cryptography.X509Certificates
{
    public enum StoreName
    {
        AddressBook = 1, // other people.
        AuthRoot = 2, // third party trusted roots.
        CertificateAuthority = 3, // intermediate CAs.
        Disallowed = 4, // revoked certificates.
        My = 5, // personal certificates.
        Root = 6, // trusted root CAs.
        TrustedPeople = 7, // trusted people (used in EFS).
        TrustedPublisher = 8, // trusted publishers (used in Authenticode).
    }
}

