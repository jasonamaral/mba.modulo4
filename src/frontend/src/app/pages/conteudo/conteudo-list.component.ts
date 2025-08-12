import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CategoryModel } from './models/conteudo.model';
import { CommonModule } from '@angular/common';
import { ConteudoTypeDescriptions, ConteudoTypeEnum } from './enums/conteudo-type.enum';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { ConteudoService } from 'src/app/services/conteudo.service';
import { ConteudoAddComponent } from './conteudo-add.component';
import { ConteudoUpdateComponent } from './conteudo-update.component';


@Component({
  selector: 'app-conteudo-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './conteudo-list.component.html',
  styleUrls: ['./conteudo-list.conponent.scss'],
})


export class ConteudoListComponent implements OnInit, OnDestroy {
  categoryModel: CategoryModel[];
  displayedColumns: string[] = ['description', 'type', 'Menu'];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private cursoSevice: ConteudoService,
    private toastr: ToastrService,
    public dialog: MatDialog) { }


  ngOnInit(): void {
    this.getCategories();
  }

  getCategories() {
    this.cursoSevice.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.categoryModel = response;
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }

  getDescription(type: ConteudoTypeEnum): string {
    return ConteudoTypeDescriptions[type] || 'Unknown';
  }

  addDialog() {
    const dialogRef = this.dialog.open(ConteudoAddComponent, {
      width: '500px',
      height: '400px',
      disableClose: true,
      data: this.categoryModel
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getCategories();
        }
      })
  }

  updateDialog(row:any) {

    let category: CategoryModel = {
      categoryId: row.categoryId,
      description: row.description,
      userId: row.userId,
      type: row.type      
    };

    const dialogRef = this.dialog.open(ConteudoUpdateComponent, {
      width: '500px',
      height: '300px',
      disableClose: true,
      data: category
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getCategories();
        }
      })
  }


  deleteCategory(category: CategoryModel) {

    const dialogData = new ConfirmDialogModel('Atenção', `Confirma exclusão da categoria <b>${category.description}</b>?`);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(dialogResult => {
        if (!dialogResult) return;

        this.cursoSevice.delete(category.categoryId)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.toastr.success('Categoria excluída com sucesso.');
              this.getCategories();
            },
            error: (fail) => {
              this.toastr.error(fail.error.errors);
            }
          });
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
