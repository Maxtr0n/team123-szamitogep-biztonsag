export class DeleteDialogData {
    entityType: EntityType;
    entityId: number;
    parentCaffId: number;
    isAdminDelete: boolean;
}

export enum EntityType {
    CAFF, COMMENT, PERSON
}