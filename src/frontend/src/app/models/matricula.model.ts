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


