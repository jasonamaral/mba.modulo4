export enum ConteudoTypeEnum {
    Income = 1,
    Expense = 2
}

export const ConteudoTypeDescriptions: { [key in ConteudoTypeEnum]: string } = {
    [ConteudoTypeEnum.Income]: 'Receitas',
    [ConteudoTypeEnum.Expense]: 'Despesas',
};
