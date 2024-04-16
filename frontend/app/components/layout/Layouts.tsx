import NavigationHeader from "./NavigationHeader";

const Layout = ({ children }: { children: React.ReactNode }) => {
  return (
    <div>
      <NavigationHeader />
      {children}
    </div>
  );
};

export default Layout;
