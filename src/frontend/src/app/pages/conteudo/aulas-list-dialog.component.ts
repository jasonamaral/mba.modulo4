import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'src/app/material.module';
import { AulaModel } from 'src/app/models/aula.model';
import { CursoModel } from 'src/app/models/curso.model';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { MatDialog } from '@angular/material/dialog';
import { AulaAddDialogComponent } from './aula-add-dialog.component';

interface DialogData {
  curso: CursoModel;
}

@Component({
  selector: 'app-aulas-list-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MaterialModule],
  template: `
    <h3 mat-dialog-title>Aulas de {{ data.curso.nome }}</h3>
    <mat-dialog-content class="content">
      <ng-container *ngIf="aulas?.length; else empty">
        <div class="aula" *ngFor="let a of aulas; let i = index">
          <div class="aula-header">
            <span class="ordem">#{{ a.ordem }}</span>
            <span class="nome">{{ a.nome }}</span>
            <span class="duracao" *ngIf="a.duracaoMinutos !== undefined">{{ a.duracaoMinutos }} min</span>
          </div>
          <div class="descricao" *ngIf="a.descricao">{{ a.descricao }}</div>
          <div class="meta">
            <span *ngIf="a.videoUrl">
              Vídeo: <a [href]="a.videoUrl" target="_blank" rel="noopener">abrir</a>
            </span>
            <span *ngIf="a.status">Status: {{ a.status }}</span>
          </div>
          <div class="actions bottom" *ngIf="isUserAdmin">
            <button mat-stroked-button color="primary" (click)="editarAula(a)">editar</button>
          </div>
        </div>
      </ng-container>
      <ng-template #empty>
        <p>Nenhuma aula cadastrada para este curso.</p>
      </ng-template>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-raised-button mat-dialog-close color="primary">Fechar</button>
    </mat-dialog-actions>
  `,
  styles: [
    `.content{max-height:70vh;min-width:300px;display:block}`,
    `.aula{padding:12px 0;border-bottom:1px solid rgba(0,0,0,.08)}`,
    `.aula:last-child{border-bottom:none}`,
    `.aula-header{display:flex;gap:12px;align-items:center;justify-content:space-between;flex-wrap:wrap}`,
    `.ordem{font-weight:600;color:#666}`,
    `.nome{font-weight:600;flex:1 1 auto}`,
    `.duracao{color:#666}`,
    `.descricao{margin:6px 0}`,
    `.meta{display:flex;gap:16px;color:#555}`,
    `.aula{display:flex;flex-direction:column}`,
    `.actions{margin-top:12px}`,
    `.actions.bottom{align-self:flex-start}`
  ]
})
export class AulasListDialogComponent {
  aulas: (AulaModel & { status?: string })[] = [];
  isUserAdmin = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private dialogRef: MatDialogRef<AulasListDialogComponent>,
    private dialog: MatDialog,
  ) {
    this.aulas = (data.curso.aulas || []) as (AulaModel & { status?: string })[];
    this.isUserAdmin = new LocalStorageUtils().isUserAdmin();
  }

  close(): void {
    this.dialogRef.close();
  }

  editarAula(aula: AulaModel): void {
    // Reaproveita o diálogo atual preenchendo os campos com os dados da aula
    const ref = this.dialog.open(AulaAddDialogComponent, {
      width: '720px',
      maxWidth: '95vw',
      disableClose: true,
      autoFocus: false,
      data: { cursoId: aula.cursoId, cursoNome: this.data.curso.nome, aula }
    });

    ref.afterClosed().subscribe(() => {
      // Em uma evolução, poderíamos recarregar a lista ao concluir uma edição real.
    });
  }
}


