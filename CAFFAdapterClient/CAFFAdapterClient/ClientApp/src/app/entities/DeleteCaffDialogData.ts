export class DeleteDialogData {
    entityType: EntityType;
    entityId: number;
    parentCaffId: number;    
}

export enum EntityType {
    CAFF, COMMENT, PERSON
}