// <copyright file="ModalDialogName.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different names of the modal dialog.
    /// </summary>
    public enum ModalDialogName
    {
        None,
        SaveMorphEntry,
        DeleteMorphEntry,
        MorphEntryIsExist,
        SaveMorphEntryError,
        DeleteMorphEntryError,
        MorphEntryInserted,
        MorphEntryUpdated,
        MorphEntryDeleted,
        MorphEntryNotFound
    }
}
