export interface DataModel {
  nodes: Node[],
  edges: Edge[]
}

// {"id":3,"label":"0.0","nodeType":"Literal"}
export interface Node {
  id: number,
  label: string,
  nodeType: string
}

// {"from":0,"to":15,"label":"tr-curr:subUnitX"}
export interface Edge {
  from: number,
  to: number,
  label: string
}

export interface VisNode extends Node {
  id: number;
  label: string;
  nodeType: string;
  color: any;
}
