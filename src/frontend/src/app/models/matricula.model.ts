export interface MatriculaCreateModel {
  alunoId: string;
  cursoId: string;
  observacao?: string;
}

export interface MatriculaModel {
  id: string;
  alunoId: string;
  cursoId: string;
  cursoNome: string;
  dataMatricula: string;
  status: string;
  percentualConclusao: number;
  dataConclusao?: string;
}


