import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { CertificadosService, CertificadoDto } from '../../../services/certificados.service';

@Component({
  standalone: true,
  selector: 'app-certificados',
  templateUrl: './certificados.component.html',
  imports: [CommonModule, MatTableModule, MatButtonModule]
})
export class CertificadosComponent {
  cols = ['curso','codigo','data','acoes'];
  certificados: CertificadoDto[] = [];
  constructor(private service: CertificadosService) {}
  ngOnInit() { this.service.listar().subscribe(d => this.certificados = d); }
  baixar(c: CertificadoDto) { window.open(c.url, '_blank'); }
}


